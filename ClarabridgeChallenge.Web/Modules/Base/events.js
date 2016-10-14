var appEvents = (function () {
    return {
        messageBox: {
            showErrorMessage: "messageBox:showError",
            showSuccessMessage: "messageBox:showSuccess"
        },
        pressRelease: {

            addUpdateDeleteShowDialog: "pressReleaseAddUpdateDelete:show",
            detailShowDialog: "pressReleaseDetail:show",
            listRefresh: "pressReleaseList:refresh"
        }
    };
}());
