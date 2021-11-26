mergeInto(LibraryManager.library, {
    SendBoxID: function(boxid) {
        dispatchReactUnityEvent("SendBoxID", boxid);
    }
    SendBoxContents: function(boxContents) {
        dispatchReactUnityEvent("SendBoxContents", boxContents);
    }
    SendBoxDimensions: function(x, y, z) {
        dispatchReactUnityEvent("SendBoxDimensions", x, y, z);
    }
})