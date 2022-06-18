package configs

import "fmt"

type AppConfig struct {
	MongoConnectionUri string
	Database           string
	Collection         string
}

var DefaultConfigs = map[string]AppConfig{
	"dev": {
		MongoConnectionUri: "mongodb://root:155202Asd...@143.198.137.31:27017",
		Database:           "CustomerServiceDb",
		Collection:         "CustomerCollection",
	},
}

var config AppConfig

func SetConfig(envName string) {
	fmt.Println(envName)
	tempConfig, exist := DefaultConfigs[envName]
	if !exist {
		panic("Invalid environment")
	}
	config = tempConfig
}

func GetConfig() *AppConfig {
	return &config
}
