REST_DIR ?= Rest-Sealious

.PHONY: all run-server deps

all: run-server run-mongo-base

run-server: deps
	cd $(REST_DIR); node . 2>&1 | tee log.txt

run-mongo-base:
	mkdir -p mongo
	mongod --dbpath mongo/ &

deps: $(REST_DIR)/package.json
	cd $(REST_DIR); sudo npm link

clean:
	rm -rf log.txt
	cd $(REST_DIR)
	rm -rf node_modules
	rm -rf uploaded_files
