var airconsole = new AirConsole({"orientation":"landscape"});

var showView = function(view_id, color) {
    var view = document.getElementById(view_id);
    var all_views = document.querySelectorAll('view');

    // Hide all containers
    for (var i=0; i<all_views.length; i++) {
        all_views[i].style.display = 'none';
    }
    
    // Show container
    view.style.display = 'inline-block';
    view.style.backgroundColor = color;
};

airconsole.onMessage = function(from, data) {
    if (data.view) {
        if (data.color) {
            showView(data.view, data.color);
        } else {
            showView(data.view);
        }
    }
    console.log("onMessage", from, data);
    
};

new Button("player-a-accel", {
    "down": function () {
        airconsole.message(AirConsole.SCREEN, {"action": "accel"});
    }
});

new Button("player-a-reverse", {
    "down": function () {
        airconsole.message(AirConsole.SCREEN, {"action": "reverse"});
    }
});

new Button("player-a-cannon-left", {
    "down": function () {
        airconsole.message(AirConsole.SCREEN, {"action": "cannon-left"});
    }
});

new Button("player-a-cannon-right", {
    "down": function () {
        airconsole.message(AirConsole.SCREEN, {"action": "cannon-right"});
    }
});

new Button("player-b-turn-left", {
    "down": function () {
        airconsole.message(AirConsole.SCREEN, {"action": "tank-left"});
    }
});

new Button("player-b-turn-right", {
    "down": function () {
        airconsole.message(AirConsole.SCREEN, {"action": "tank-right"});
    }
});

new Button("player-b-shoot", {
    "down": function () {
        airconsole.message(AirConsole.SCREEN, {"action": "shoot"});
    }
});