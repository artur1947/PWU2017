"use strict";

const Sealious = require("sealious");

module.exports = function(app){
	return app.createChip(Sealious.AccessStrategyType, {
		name: "roles",
		checker_function: function(context, params, item){
			if(context.user_id === null){
				return Promise.reject("Musisz być zalogowany");
			}

			const user_id = context.user_id;

			return app.run_action(
				new app.Sealious.SuperContext(),
				["collections", "users", user_id],
				"show",
				{format: {role: "original"}}
			)
			.then(function(user){
				if(params.indexOf(user.body.role)!==-1){
					return Promise.resolve();
				}else{
				    return Promise.reject("Brak uprawnień do wykonania tej akcji. Akcja dozwolona tylko dla: " + params.join(", ") + ".");
				}
			});
		}
	});
};
