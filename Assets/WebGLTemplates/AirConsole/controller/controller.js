var airconsole = new AirConsole({"orientation":"landscape"});

            airconsole.onMessage = function(from, data) {
                if (data.view) {
                    showView(data.view);
                }
                console.log("onMessage", from, data);
                
            };

            var showView = function(id) {
                var view = document.getElementById(id);
                var all_views = document.querySelectorAll('view');

                // Hide all containers
                for (var i=0; i<all_views.length; i++) {
                    all_views[i].style.display = 'none';
                }
                
                // Show container
                view.style.display = 'inline-block';
            };

            new DPad("dpad", {
                "directionchange": function(key, pressed) {
                airconsole.message(AirConsole.SCREEN, {
                    "dpad": {
                    "directionchange": {
                        "key": key,
                        "pressed": pressed
                    }
                    }
                });
                },
                "touchstart": function() {
                    airconsole.message(AirConsole.SCREEN, {
                    "dpad": {
                        "touch": true
                    }
                    });
                },
                "touchend": function(had_direction) {
                    airconsole.message(AirConsole.SCREEN, {
                    "dpad": {
                        "touch": false,
                        "had_direction": had_direction
                    }
                    });
                }
            });

            new Button("a", {
                "down": function () {
                    airconsole.message(AirConsole.SCREEN, {"a": "down"});
                },
                "up": function () {
                    airconsole.message(AirConsole.SCREEN, {"a": "up"});
                }
            });

            new Button("b", {
                "down": function () {
                    airconsole.message(AirConsole.SCREEN, {"b": "down"});
                },
                "up": function () {
                    airconsole.message(AirConsole.SCREEN, {"b": "up"});
                }
            });
            
            new Button("c", {
                "down": function () {
                    airconsole.message(AirConsole.SCREEN, {"c": "down"});
                },
                "up": function () {
                    airconsole.message(AirConsole.SCREEN, {"c": "up"});
                }
            });
            
            new Button("d", {
                "down": function () {
                    airconsole.message(AirConsole.SCREEN, {"d": "down"});
                },
                "up": function () {
                    airconsole.message(AirConsole.SCREEN, {"d": "up"});
                }
            });