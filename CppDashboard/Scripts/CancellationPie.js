(function(w) {
    w.createCancellationPie = function(orpahans, totalcancellations) {
        var excludingOrpahans = totalcancellations - orpahans;
        var options = {
            "header": {
                "title": {
                    "fontSize": 24,
                    "font": "open sans"
                },
                "subtitle": {
                    "color": "#999999",
                    "fontSize": 12,
                    "font": "open sans"
                },
                "titleSubtitlePadding": 9
            },
            "footer": {
                "color": "#999999",
                "fontSize": 10,
                "font": "open sans",
                "location": "bottom-left"
            },
            "size": {
                "canvasWidth": 200,
                "pieOuterRadius": "90%"
            },
            "data": {
                "sortOrder": "random",
                "content": [
                    {
                        "label": "Orphans",
                        "value": orpahans,
                        "color": "#8b6834"
                    },
                    {
                        "label": "Cancelled",
                        "value": excludingOrpahans,
                        "color": "#228835"
                    }
                ]
            },
            "labels": {
                "outer": {
                    "format": "none",
                    "pieDistance": 10
                },
                "inner": {
                    "format": "label-percentage2",
                    "hideWhenLessThanPercentage": 3
                },
                "mainLabel": {
                    "fontSize": 11
                },
                "percentage": {
                    "color": "#ffffff",
                    "decimalPlaces": 0
                },
                "value": {
                    "color": "#ffffff",
                    "fontSize": 11
                },
                "lines": {
                    "enabled": true
                },
                "truncation": {
                    "enabled": true
                }
            },
            "effects": {
                "pullOutSegmentOnClick": {
                    "effect": "linear",
                    "speed": 400,
                    "size": 8
                }
            },
            "misc": {
                "gradient": {
                    "enabled": true,
                    "percentage": 100
                }
            }
        };

        $('#cancellationPie')[0].innerHTML = '';

        var cancellationPie = new d3pie("cancellationPie", options);
        return cancellationPie;
    };
})(window);

