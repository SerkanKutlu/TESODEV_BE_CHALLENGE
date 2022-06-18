package main

import (
	"github.com/SerkanKutlu/CustomerService/configs"
	_ "github.com/SerkanKutlu/CustomerService/docs"
	"github.com/SerkanKutlu/CustomerService/internal"
	"github.com/labstack/echo/v4"
	echoSwagger "github.com/swaggo/echo-swagger"
	"os"
)

//@title User API Documentation
//@version 1.0.0

//@host 143.198.137.31:5000
//@basePath /api
func main() {

	env := os.Getenv("GO_ENV")
	configs.SetConfig(env)

	config := configs.GetConfig()

	client, err := internal.GetClient(config.MongoConnectionUri)
	if err != nil {

		panic(err.Error())
	}
	collection, _ := internal.GetCollection(client, config.Database, config.Collection)
	repo := internal.NewCustomerRepository(collection)
	handler := internal.NewHandler(repo)
	e := echo.New()
	e.GET("/api/customer", handler.GetAll)
	e.GET("/api/customer/:id", handler.GetById)
	e.GET("/api/customer/validate/:id", handler.Validate)
	e.DELETE("/api/customer/:id", handler.Delete)
	e.POST("/api/customer", handler.Post)
	e.PUT("/api/customer", handler.Put)
	e.GET("/swagger/*", echoSwagger.WrapHandler)
	e.Start(":5000")
}
