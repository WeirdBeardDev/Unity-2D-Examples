# Unity 2D Examples
I routinely work blog about my Unity 2D work and decided I wanted to share my examples.

If you have any questions or even better suggestions for improvement let me know.

You can find my blog at [WeirdBeardDev](https://weirdbearddev.com).

---

# SpaceMonkeys.Core
`FindInParents<T>` is a GameObject extenions method that allows you to find a component in the parent hierarchy.  For example, you can find the Canvas component of a GameObject multiple levels deep in the hierarchy.

# SpaceMonkeys.UI
`DragWindow` is a script that you drop on a Header of a GameObject you want to move around.  Features include:
* making the content area transparent while moving
* clamping to screen (default)
* close the window if part of it outside the Canvas
  * applying an offset so the window can go offscreen by the offset amount

![DragWindow](https://dl.dropbox.com/s/emvr52bpolg5zv2/DragWindow-1.png?dl=0)