# Rect Readonly Record Struct Plan

## Goal

Change `Rect` from a mutable reference type into an immutable value object:

```csharp
public readonly record struct Rect(int X, int Y, int Width, int Height);
```

The main reason to make this change is that a rectangle is geometry data, not an identity-bearing object. Two rectangles with the same coordinates and dimensions should compare equal, and callers should not need to care whether they are the same instance.

## Current compatibility risks

The current `Rect` is a mutable class. Existing widget code mutates the current `BoundingBox` in place:

- `Move()` changes `BoundingBox.X` and `BoundingBox.Y`
- `Nudge()` increments `BoundingBox.X` and `BoundingBox.Y`
- `Resize()` changes `BoundingBox.Width` and `BoundingBox.Height`

That pattern will not work the same way with a readonly struct. The migration should update these methods to replace the entire `BoundingBox` value instead of mutating individual properties.

There is also a default-value difference. Today, `new Rect()` creates `X = -1`, `Y = -1`, `Width = 0`, `Height = 0`. A struct default is always `0, 0, 0, 0`. If the `-1, -1` sentinel matters, it should become an explicit named value.

## Recommended shape

Use a positional readonly record struct for the normal constructor and generated value equality:

```csharp
public readonly record struct Rect(int X, int Y, int Width, int Height) {
    public static Rect Empty() {
        return new Rect(-1, -1, 0, 0);
    }

    public bool IsEmpty() {
        return Width == 0 || Height == 0 || X < 0 || Y < 0;
    }
}
```

If `Rect.Empty()` feels inconsistent with other types, `Rect.Empty` as a static property is also reasonable:

```csharp
public static Rect Empty => new Rect(-1, -1, 0, 0);
```

## Migration steps

1. Add focused tests for value equality before changing the implementation.
2. Add tests that document the intended empty rectangle behavior.
3. Convert `Rect` to a `readonly record struct`.
4. Replace `new Rect()` call sites with `Rect.Empty()` or `Rect.Empty`.
5. Update widget mutation methods to assign whole values:

```csharp
public void Move(int x, int y) {
    BoundingBox = BoundingBox with { X = x, Y = y };
}

public void Nudge(int xAmount, int yAmount) {
    BoundingBox = BoundingBox with {
        X = BoundingBox.X + xAmount,
        Y = BoundingBox.Y + yAmount,
    };
}

public void Resize(int width, int height) {
    BoundingBox = BoundingBox with { Width = width, Height = height };
}
```

6. Keep `Widget.SetBounds(Rect rect)` unchanged unless dirty tracking needs stricter behavior. `rect == BoundingBox` will become value equality automatically.
7. Run the full test suite and add layout/widget tests for any behavior that changes.

## Recommendation

Do this as a small, explicit API migration rather than a casual equality cleanup. The value equality part is easy, but the important behavioral change is immutability. Make the default and empty-rectangle semantics explicit first, then update all mutation sites to replace `Rect` values atomically.
