if (typeof(findAvailablePortProcessInfo) != "undefined") {
    findAvailablePortProcessInfo.kill();
}

function findAvailablePortAndExecFunc(callBackFunc) {

    findAvailablePortProcessInfo = hidemaru.runProcess(findAvailablePortAndExecFunc.currentMacroDirectory + "\\HmAvailablePort.exe", ".", "stdio", "utf8");
    var stdOut = findAvailablePortProcessInfo.stdOut;
    stdOut.onReadAll(readAllAsync);

    function readAllAsync(outline) {
        try {
            var match = /^(\d+)/.exec(outline);
            if (match) {
                var port = parseInt(match[1], 10);
                if (callBackFunc) {
                    callBackFunc(port);
                }
            } else {
                    callBackFunc(0);
            }
        } catch(e) {
        } finally {
            if (findAvailablePortProcessInfo) {
                findAvailablePortProcessInfo.kill();
            }
        }
    }
}

findAvailablePortAndExecFunc.currentMacroDirectory = currentmacrodirectory();