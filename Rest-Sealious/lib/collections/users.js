const Sealious = require("sealious");

module.exports = function(app){
	var Users = app.ChipManager.get_chip("collection", "users");

	Users.add_fields([
		{name: "role", type: "role", required: true},
	]);

	Users.set_access_strategy({
		create: "public",
		retrieve: ["or", ["themselves", ["roles", ["admin"]]]],
		update: ["or", ["themselves" , ["roles", ["admin"]]]],
		delete: "noone",
	});
};
