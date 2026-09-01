# Multiline TextBox Plan

## Summary

Build a basic multiline editor with logical lines, caret navigation, horizontal and vertical scrolling, and terminal cursor synchronization. Keep selection, clipboard, undo/redo, overwrite mode, soft wrapping, word movement, page
movement, and full Unicode cell-width handling out of the first release.

The existing architecture is directionally aligned: input is separated into text and key events, input follows focus, widgets expose cursor state, and terminal cursor placement occurs after rendering. The current 131 tests pass,
but none cover TextBox, CursorManager, or InputParser, so the unfinished behavior is effectively unverified.

## Current Work Assessment

- Keep the TextInputEvent versus KeyEvent distinction. This is the appropriate boundary between typed content and editing commands.
- Keep FocusManager as the authority for keyboard focus and CursorManager as the authority for the physical terminal cursor.
- Keep text as logical lines, but move line mutation and logical caret movement out of the 444-line widget into a small internal, UI-independent editor state.
- Treat the focused widget as the cursor owner. Widgets without a meaningful cursor expose an invisible cursor; a separate cursor-ownership system is unnecessary now.
- Correct the current blockers: TextBox.AcceptsFocus is false, cursor validation rejects every positive column, several deletion ranges are incorrect, edits do not consistently move the caret or dirty the widget, newline handling
is incomplete, and SetContent can strand the caret.

- Separate the logical caret from its displayed cursor. The current cursor position cannot simultaneously represent a document coordinate and a widget-relative screen coordinate once scrolling, borders, or padding are involved.
- Stop using TextBuffer.FromLines for editable content because it soft-wraps text while the editor state assumes one display row per logical line.
- Split targeted event delivery from tree broadcasts. Keyboard input should go only to the focused widget and optionally bubble through its ancestors; resize events should broadcast downward. The current focused-target dispatch
can incorrectly send input into descendants.

## Implementation Changes

### Editing state and commands

- Add an internal editor-state type that owns normalized logical lines, a zero-based caret (column, line), and the preferred column used by vertical movement.
- Maintain at least one logical line at all times; empty content is represented by one empty line.
- Support typed strings, including normalized \n, \r\n, and \r, inserting them at the caret and advancing it to the end of the inserted content.
- Implement Enter by splitting the current line.
- Implement Backspace by deleting the previous character or joining with the previous line at column zero.
- Implement Delete by deleting the next character or joining with the next line at line end.
- Implement Left/Right across line boundaries, Up/Down with preferred-column preservation, Home/End for line boundaries, and Ctrl+Home/Ctrl+End for document boundaries.
- Insert spaces through the next four-column tab stop for plain Tab. TextBox captures Tab; Escape continues to move focus to the root, after which normal Tab traversal resumes.
- Leave PageUp/PageDown, word movement, Insert/overwrite, selection modifiers, clipboard commands, and undo/redo unhandled.
- Make all successful mutations dirty the widget. Navigation updates the viewport and displayed cursor without requiring text-buffer regeneration unless the viewport changes.
- Make SetContent normalize content and reset the caret and viewport to the document start. Make SetStyle dirty the widget.
- Keep read-only text boxes focusable for navigation and scrolling, but ignore mutation commands and do not capture Tab.

### Viewport and painting

- Store zero-based top-line and left-column viewport offsets in TextBox.
- Do not soft-wrap. Render one logical line per content-area row and slice each line from the horizontal viewport.
- After every caret movement, edit, resize, or content reset, adjust the viewport minimally so the caret remains visible.
- Render only inside ContentArea(), preserving border and padding cells.
- Convert the logical caret to a widget-local displayed cursor by adding the content-area inset and subtracting viewport offsets.
- Hide the cursor when the widget lacks focus, its content area is empty, or the displayed caret is outside the drawable area.
- Continue treating one UTF-16 char as one terminal cell, matching the current Cell and TextBuffer model. Wide and combining-character support remains a later cross-library concern.

### Focus, dispatch, and physical cursor

- Set editable TextBox instances to accept focus and ensure focus transitions update HasFocus consistently.
- Preserve IWidget.GetCursor() and the existing Cursor type. Default widget cursors to invisible; TextBox updates its cursor visibility from focus and geometry.
- Give event dispatch separate targeted and broadcast operations: target keyboard/text events at the focused widget, and broadcast resize events through the tree.
- Mark text and key events handled only when TextBox actually processes them.
- Keep CursorManager subscribed to focus changes and have it read the owner’s current cursor each frame.
- Define cursor positions as zero-based inside the library. CursorManager adds the widget bounding-box origin; the textbox already includes its content inset.
- Convert to ANSI’s one-based coordinates only when emitting MoveCursorTo.
- Honor cursor visibility and style, add a hide-cursor sequence, and hide rather than clamp a cursor outside the current screen.
- Initialize cursor-manager screen dimensions before the first frame and restore a visible default cursor when the application exits.
- Defer a complete terminal-driver abstraction; keep terminal I/O localized to the existing renderer, input parser, application, and cursor manager.

## Test Plan

- Add pure editor-state tests for insertion at every position, multiline insertion, newline normalization, line splitting/joining, boundary deletion, empty content, and caret validity after content replacement.
- Test horizontal and vertical movement, cross-line movement, preferred columns on uneven lines, Home/End, Ctrl+Home/Ctrl+End, and movement at document boundaries.
- Test four-column Tab expansion and read-only mutation rejection.
- Add TextBox tests for focusability, event handling, dirty state, clipping without wrapping, horizontal/vertical viewport following, borders/padding, empty content areas, and displayed cursor coordinates.
- Add CursorManager tests for widget offsets, ANSI one-based conversion, visibility, style output, screen clipping, focus changes, and resize handling.
- Add dispatcher tests proving targeted input does not descend into children while resize broadcasts do.
- Add parser classification tests for printable characters, control characters, and special keys.
- Add an interactive textbox to the demo for manual verification of typing, multiline editing, scrolling, Escape, focus traversal, resizing, and terminal cursor restoration.
- Run dotnet test QuickTui.sln -nodeReuse:false -maxcpucount:1 after each subsystem and at completion.

## Assumptions

- The first release is a multiline editor with horizontal scrolling and no soft wrapping.
- Tab inserts spaces through a four-column tab stop; Escape is the mechanism for leaving the editor before focus traversal.
- Mouse input, password masking, validation, submission callbacks, selection, clipboard integration, undo history, rich text, and advanced Unicode rendering are deferred.
- No new external packages are required.
