# Widget Tree Refactor Plan

## Current approach

The current model keeps widget tree management simple:

- every widget can have a `Parent`
- every widget can have children
- `Parent` is public and mutable
- widget constructors accept a parent, including `null`
- passing a parent in the constructor immediately attaches the widget to that parent

This is a reasonable early-stage approach because it keeps the API small and lets the tree be built incrementally without much ceremony.

This is also closer to the construction style used by Qt and Tkinter than the earlier version of the design. Both frameworks encourage building the widget tree progressively by passing the parent at construction time.

## Why this may become painful later

As the UI library grows, the current model will make a few classes of bugs easier to introduce:

- parent and child state can drift apart because both sides are mutable
- a widget can be attached to one parent but still appear in another parent's `_children`
- non-container widgets expose child-management APIs even if they do not meaningfully own children
- reparenting policy is undefined, so moving widgets around the tree may behave inconsistently
- external application code can mutate `Parent` directly and bypass any tree invariants

These issues are manageable while the library is small, but they tend to get more expensive once rendering, focus management, event routing, and layout depend on a trustworthy tree.

The current implementation also introduces one constructor-ordering risk: a child attaches to its parent before the child has finished initializing itself. That is tolerable if the parent only stores the reference, but it becomes fragile if parent-side logic later depends on the child being fully initialized.

## Refactor goal

Move toward a stricter widget tree where:

- the tree has one authoritative attachment path
- parent and child relationships are always updated together
- only widgets that actually own children expose child-management operations
- application code can observe the tree but cannot corrupt it directly

## Recommended staged refactor

### Stage 1: Keep the constructor ergonomics

Preserve the ability to construct widgets with a parent parameter.

That is a reasonable fit with Qt and Tkinter, and it supports the progressive tree-building style you want.

The refinement to make later is not "remove parent from constructors." It is:

- finish the widget's own initialization first
- then, if a parent is supplied, attach through one authoritative attach path

That keeps the progressive tree-building style while reducing partially-initialized-child risks.

### Stage 2: Split leaf and parent responsibilities

Remove `AddChild()` and `GetChildren()` from `IWidget`.

Introduce a dedicated parent-capable interface, for example:

```csharp
public interface IParentWidget : IWidget {
    IReadOnlyList<IWidget> GetChildren();
    void AddChild(IWidget child);
}
```

If the current architecture stays container-oriented, this could simply be `IContainer`.

This avoids forcing leaf widgets like `Label` to carry child-management APIs they do not need.

If staying close to Qt matters more than enforcing a strict container-only model, you may choose to skip this stage and continue allowing any widget to own children. Qt is broadly compatible with that model. Tkinter is also permissive in practice, even though container-like widgets tend to dominate real layouts.

### Stage 3: Make parent observation public, mutation internal

Change:

```csharp
public IWidget? Parent { get; set; }
```

to:

```csharp
public IWidget? Parent { get; }
```

and add internal mutation helpers on the base widget, for example:

```csharp
internal void AttachTo(IWidget parent)
internal void DetachFrom(IWidget parent)
```

That allows the library to maintain the tree while preventing client apps from mutating it arbitrarily.

If matching Qt/Tkinter ergonomics remains a priority, keep constructor parent overloads public while still making post-construction parent mutation internal.

### Stage 4: Centralize attachment policy

Make `AddChild()` the authoritative public operation for tree mutation.

It should:

- reject `null`
- reject adding a widget to itself
- reject cycles
- reject or explicitly reparent children already attached elsewhere
- update both the parent collection and the child's `Parent`

Define reparenting policy explicitly:

- strict option: throw if `child.Parent` is not `null`
- flexible option: remove from previous parent before attaching

Either is valid, but it should be consistent across the library.

If you want to stay closer to Qt, the flexible reparenting option is the more natural long-term fit.

### Stage 5: Add removal support

Add:

- `RemoveChild(IWidget child)`
- possibly `ClearChildren()`

Without removal support, any later attempt to enforce tree invariants will stay incomplete.

### Stage 6: Add focused tests for tree integrity

Add tests for:

- constructor with `null` parent leaves widget unattached
- constructor with parent routes through `AddChild()`
- adding a child updates both parent and child state
- reparenting either throws or detaches first, depending on policy
- leaf widgets cannot accept children if you split interfaces
- removing a child clears its `Parent`
- cycles are rejected

## Suggested order

1. Introduce tree integrity tests first.
2. Decide whether only containers own children, or any widget may own children.
3. Move `AddChild()` off `IWidget` if leaf widgets should stay simple.
4. Make `Parent` read-only to consumers.
5. Route constructor parent overloads through the same attach path after child initialization completes.
6. Add remove and reparent behavior.

## Recommendation

Keep the current approach for now if it is helping you move quickly. It is a reasonable starting point and it does not diverge drastically from Qt or Tkinter at the API-shape level.

The first time you add event propagation, focus tracking, or nested composite widgets, plan to tighten this up. That is the point where stale or contradictory parent/child relationships stop being a minor cleanup issue and start causing real behavioral bugs.
