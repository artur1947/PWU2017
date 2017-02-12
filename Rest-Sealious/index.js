const Sealious = require("sealious");

var App = new Sealious.App();

require("./lib/field-types/role.js")(App);
require("./lib/access-strategy/roles.js")(App);
require("./lib/collections/users.js")(App);
require("./routes.js")(App);

App.createCollection({
    name: "files",
    fields: [
        {name: "title", type: "text", required: true},
        {name: "text", type: "text", required: true},
    ],
    access_strategy: {
        default: ["roles", ["admin"]],
        retrieve: "public",
        delete: ["roles", ["admin"]]
    }
});

App.start().then(function(){
    App.run_action(
        new App.Sealious.SuperContext(),
        ["collections", "users"],
        "create",
        {username: "admin", password: "admin", role: "admin"}
    )
        .catch(function(error){
            console.error(error);
        });
});
