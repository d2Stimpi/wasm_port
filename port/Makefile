MAKEFLAGS= --no-builtin-rules
ROOT= $(realpath .)

#TYPE= normal
TYPE= wasm


EMCFLAGS_SIDE_MODULE= -s SIDE_MODULE
EMCFLAGS_MAIN_MODULE= -s MAIN_MODULE

ifeq ($(TYPE),normal)
	CC=g++
# Box2D target
	BOX2D_TARGET= libbox2d.so
# YAML target
	YAML_TARGET= libyaml-cpp.so
# Emscripten flags
	EMCFLAGS_SIDE_MODULE=
	EMCFLAGS_MAIN_MODULE=
else
	CC=emcc
# Box2D target
	BOX2D_TARGET= libbox2d.wasm
# YAML target
	YAML_TARGET= libyaml-cpp.wasm
endif

# Libraries
LIBS= $(ROOT)/$(BOX2D_TARGET) \
		$(ROOT)/$(YAML_TARGET)

BOX2D_INC := vendor/Box2D/include/
BOX2D_CFLAGS=-I$(BOX2D_INC) -fPIC -shared

BOX2D_SRCFILES := $(wildcard vendor/Box2D/src/*/*.cpp)
BOX2D_FILES := $(patsubst %.cpp,%.o,$(BOX2D_SRCFILES))

all: box2d yaml-cpp mainapp

# Build the box2d library (SIDE_MODULE)
box2d: $(BOX2D_FILES)
	@echo "Building box2d..."
	$(CC) -o $(BOX2D_TARGET) $(BOX2D_FILES) $(BOX2D_CFLAGS) $(EMCFLAGS_SIDE_MODULE)


YAML_INC := vendor/yaml-cpp/include/
YAML_CFLAGS=-I$(YAML_INC) -fPIC -shared -DYAML_CPP_STATIC_DEFINE

YAML_SRCFILES := $(wildcard vendor/yaml-cpp/src/*.cpp)
YAML_FILES := $(patsubst %.cpp,%.o,$(YAML_SRCFILES))
# Build the yaml-cpp library
yaml-cpp: $(YAML_FILES)
	@echo "Building yaml-cpp..."
	$(CC) -o $(YAML_TARGET) $(YAML_FILES) $(YAML_CFLAGS) $(EMCFLAGS_SIDE_MODULE)


# Common object compilation rule
%.o : %.cpp
	@echo "Compiling $<..."
	$(CC) -c $< -o $@ $(BOX2D_CFLAGS) $(YAML_CFLAGS)

app: mainapp

# Build the mainapp (MAIN_MODULE)
mainapp: main.cpp
	@echo "Building mainapp..."
	@echo $(ROOT)
	$(CC) -o mainapp $(EMCFLAGS_MAIN_MODULE) main.cpp $(LIBS) wasm_test.wasm -I$(BOX2D_INC) -I$(YAML_INC)


install: main.cpp
	@echo "Installing..."
	emcc -sMAIN_MODULE main.cpp $(BOX2D_TARGET) $(YAML_TARGET) -I$(BOX2D_INC) -I$(YAML_INC) -o test.html
	mkdir -p out
	mv test* out/
	cp $(BOX2D_TARGET) out/
	cp $(YAML_TARGET) out/



clean:
	@echo "Cleaning up..."
	rm -rf vendor/Box2D/src/*/*.o
	rm -rf vendor/yaml-cpp/src/*.o
	rm -rf $(BOX2D_TARGET)
	rm -rf $(YAML_TARGET)
	rm -rf mainapp*
	rm -rf libbox2d.so

clean_all: clean
	rm -rf out
