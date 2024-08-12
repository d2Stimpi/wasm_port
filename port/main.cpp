#include <iostream>
#include <fstream>

#include "box2d/box2d.h"
#include "yaml-cpp/yaml.h"

b2World* g_World;
b2Body* g_Body;

void InitWorld()
{
    b2Vec2 gravity(0.0f, -10.0f);
    g_World = new b2World(gravity);
}

void PopulateWorld()
{
    // Static body
    b2BodyDef groundBodyDef;
    groundBodyDef.position.Set(0.0f, -10.0f);
    b2Body* groundBody = g_World->CreateBody(&groundBodyDef);
    b2PolygonShape groundBox;
    groundBox.SetAsBox(50.0f, 10.0f);
    groundBody->CreateFixture(&groundBox, 0.0f);

    // Dynamic body
    b2BodyDef bodyDef;
	bodyDef.type = b2_dynamicBody;
	bodyDef.position.Set(0.0f, 4.0f);
	b2Body* body = g_World->CreateBody(&bodyDef);
    // Define another box shape for our dynamic body.
	b2PolygonShape dynamicBox;
	dynamicBox.SetAsBox(1.0f, 1.0f);
    // Define the dynamic body fixture.
	b2FixtureDef fixtureDef;
	fixtureDef.shape = &dynamicBox;
    fixtureDef.density = 1.0f;
    fixtureDef.friction = 0.3f;
    body->CreateFixture(&fixtureDef);

    g_Body = body;
}

void Simulate()
{
    float timeStep = 1.0f / 60.0f;
	int32 velocityIterations = 6;
	int32 positionIterations = 2;

    b2Vec2 position = g_Body->GetPosition();
	float angle = g_Body->GetAngle();

    for (int32 i = 0; i < 60; ++i)
	{
		// Instruct the world to perform a single step of simulation.
		// It is generally best to keep the time step and iterations fixed.
		g_World->Step(timeStep, velocityIterations, positionIterations);

		// Now print the position and angle of the body.
		position = g_Body->GetPosition();
		angle = g_Body->GetAngle();

        std::cout << position.x << " " << position.y << " " << angle << std::endl;
	}

    g_World->Step(1.0f / 60.0f, 6, 2);
}

void WriteSampleYamlFile(const char* fileName)
{
    YAML::Emitter out;

	out << YAML::Block;
	out << YAML::BeginMap;
    {
        out << YAML::Key << "Project" << YAML::Value << "WASM Port";
	}
	out << YAML::EndMap;

	std::ofstream outFile;
	outFile.open(fileName);
	outFile << out.c_str() << std::endl;
	outFile.close();
}

void ReadSampleYamlFile(const char* fileName)
{
    YAML::Node loadData = YAML::LoadFile(fileName);
    if (loadData.IsNull())
    {
        std::cout << "Error loading yaml file: " << fileName << std::endl;
        return;
    }

    std::cout << "Read data from " << fileName << ":" << std::endl;
    std::cout << loadData["Project"].as<std::string>() << std::endl;
}

extern int Answer();

int main(int argc, char** argv)
{
    std::cout << "Emscripten - Wasm - Box2D example" << std::endl;

    WriteSampleYamlFile("project.yaml");
    ReadSampleYamlFile("project.yaml");

    InitWorld();

    if(g_World == nullptr)
    {
        std::cout << "Error creating world" << std::endl;
        return 1;
    }

    PopulateWorld();
    Simulate();

    std::cout << "And the answer is: " << Answer() << std::endl;

    delete g_World;
    g_World = nullptr;
    
    return 0;
}