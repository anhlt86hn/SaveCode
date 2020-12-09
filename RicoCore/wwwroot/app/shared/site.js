(function () {
    "use strict";

    var scoreHub;
    var $alertMsg;
    var alertHtmlClose = '<a href="#" data-dismiss="alert" class="close">&times;</a>';
    var alertHtml;

    $(function () {
        $.ajaxSetup({
            cache: false
        });

        scoreHub = $.connection.ricoHub;

        configListeners();

        // Start Hub
        $.connection.hub.start({ transport: ['webSockets', 'serverSentEvents', 'longPolling'] })
            .done(function () {
                scoreHub.server.connect();
            });
    });

    function configListeners() {
        scoreHub.client.getResult = function (liveScore) {
            $alertMsg.hide();
            if (liveScore.TeamName1 == null) {
                if (!$("#Team1").length) {
                    return false;
                }
                $("#objectToAnimate").hide();
                $alertMsg.show();
                return false;
            }

            $("#Team1").html(liveScore.TeamName1);
            $("#Team2").html(liveScore.TeamName2);

            $("#Score1").html(liveScore.Team1Score);
            $("#Score2").html(liveScore.Team2Score);

            $("#objectToAnimate").fadeOut(500);
            $("#objectToAnimate").fadeIn(500);
        };

        $alertMsg = $("#alertMsg");
        $alertMsg.hide();
        $alertMsg.on("close.bs.alert", function () {
            $alertMsg.hide();
            return false;
        });
    }

})();