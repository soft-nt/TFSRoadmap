$().ready(function () {

    // Set callback to run when API is loaded
    $.ajax({
        url: "/api/roadmap"
    }).done(function (result) {
        var data = [];

        $(result).each(function (index, element) {
            var risk = '';
            switch (element.Risk) {
                case '1 - High':
                    risk = 'high';
                    break;
                case '2 - Medium':
                    risk = 'medium';
                    break;
                case '3 - Low':
                    risk = 'low';
                    break;
            default:
            }

            data.push({
                'start': new Date(element.Start),
                'end': new Date(element.End),
                'content': "<div>" + element.Title + "<br><a href='" + element.Url + "' target='_blank'>" + element.Url + "</a><br>Tags: " + element.Tags + "</div>",
                'group': element.Group,
                'className': risk
            });
        });

        // specify options
        var options = {
            width: "100%",
            height: "1000px",
            axisOnTop: true,
            eventMargin: 10,  // minimal margin between events
            eventMarginAxis: 0, // minimal margin beteen events and the axis
            editable: false,
            showNavigation: true
        };

        // Instantiate our timeline object.
        var timeline = new links.Timeline(document.getElementById('mytimeline'), options);
        // Draw our timeline with the created data and options
        timeline.draw(data);
    });
    

});
