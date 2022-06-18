package configs

type AppConfig struct {
	MongoConnectionUri string
	Database           string
	Collection         string
}

var DefaultConfigs = map[string]AppConfig{ //key - value
	"dev": {
		MongoConnectionUri: "mongodb://root:155202Asd...@143.198.137.31:27017",
		Database:           "OrderServiceDb",
		Collection:         "OrderCollection",
	},
	"devProduct": {
		MongoConnectionUri: "mongodb://root:155202Asd...@143.198.137.31:27017",
		Database:           "OrderServiceDb",
		Collection:         "ProductCollection",
	},
}

var config AppConfig

func SetConfig(envName string) {
	tempConfig, exist := DefaultConfigs[envName]
	if exist == false {
		panic("Invalid environment")
	}

	config = tempConfig

}

func GetConfig() *AppConfig {
	return &config
}
