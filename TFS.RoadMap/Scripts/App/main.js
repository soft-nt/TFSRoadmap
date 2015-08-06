var roadmapApp = angular.module('roadmapApp', []);

roadmapApp.directive('timeline', function ($timeout, $parse) {
        return {
            restrict: 'E', // says that this directive is only for html elements
            replace: true,
            template: '<div id="timeline{{$id}}"></div>',
            link: function ($scope, $element, $attributes) {
                if ($attributes.ngModel) {
                    // specify options
                    var options = {
                        width: $attributes.width,
                        height: $attributes.height,
                        axisOnTop: true,
                        eventMargin: 10,  // minimal margin between events
                        eventMarginAxis: 0, // minimal margin beteen events and the axis
                        editable: false,
                        showNavigation: true
                    };

                    var timeline;

                    function onselect() {
                        var sel = timeline.getSelection();
                        if (sel.length) {
                            if (sel[0].row != undefined) {
                                var row = sel[0].row;
                                var item = timeline.getItem(row);

                                //var onSelect = $parse($attributes.ngSelect);
                                //onSelect(1);
                                //$scope.onItemSelect(1);
                                //alert("Selected tfs id: " + item.wiId);
                            }
                        }
                    }

                    $scope.$watch($attributes.ngModel, function (a, b) {
                        var data = $scope.$eval($attributes.ngModel);
                        if (data && data.length > 0) {
                            $timeout(function () {
                                timeline = new links.Timeline(document.getElementById("timeline" + $scope.$id), options);
                                links.events.addListener(timeline, 'select', onselect)
                                timeline.draw(data);
                            }, 0);
                        }
                    }, true);
                }
            }
        };
    });

roadmapApp.controller('roadmapCtl', function ($scope, roadmapService) {
    $scope.data = [];

    $scope.displayUs = function () {
        alert("Test");
    };

    $scope.load = function () {
        roadmapService.getRoadMap().then(function (result) {
            $scope.data = [];

            $(result.data).each(function (index, element) {
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

                $scope.data.push({
                    'start': new Date(element.Start),
                    'end': new Date(element.End),
                    'wiId' : element.WIId,
                    'content': "<div title='" + element.Title + " - P" + element.Priority + "'><div class='contentTitle'>" + element.Title + " -- P" + element.Priority +
                        "</div>TFS: <a href='" + element.Url + "' target='_blank'>" + element.WIId + "</a> - Status: "+element.Status+" - Active US count: <a href='#'>" + element.ChildUSCount + "</a>" +
                        "<br>Tags: " + element.Tags + "</div>",
                    'group': element.Group,
                    'className': risk
                });
            });
        }, function(error) {});
    };

    $scope.load();
});

roadmapApp.service('roadmapService', function ($http) {
    function getRoadMap() {
        var request = $http({
            method: "get",
            url: "/api/roadmap?project=PSG%20Dashboard&rootQuery=Shared%20Queries&query=Features%20for%20Roadmap"
        });
        return request;
    }

    return ({
        getRoadMap: getRoadMap
    });
});