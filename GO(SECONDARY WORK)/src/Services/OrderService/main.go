package main

import (
	"OrderService/configs"
	"OrderService/database"
	_ "OrderService/docs"
	"OrderService/handlers"
	"OrderService/repositories"
	"github.com/labstack/echo/v4"
	echoSwagger "github.com/swaggo/echo-swagger"
	"os"
)

//@title User API Documentation
//@version 1.0.0

//@host 143.198.137.31:5001
//@basePath /api
func main() {

	//For order
	env := os.Getenv("GO_ENV")
	configs.SetConfig(env)
	config := configs.GetConfig()
	client, err := database.GetClient(config.MongoConnectionUri)
	if err != nil {
		panic("Mongo is not reachable")
	}
	collection, _ := database.GetCollection(client, config.Database, config.Collection)
	repo := repositories.NewOrderRepository(collection)
	handler := handlers.NewHandler(repo)
	e := echo.New()
	e.GET("/api/order", handler.GetAll)
	e.GET("/api/order/:id", handler.GetById)
	e.GET("/api/order/customer/:id", handler.GetOrderByCustomerId)
	e.PUT("/api/order/status/:id/:status", handler.ChangeStatus)
	e.DELETE("/api/order/:id", handler.Delete)
	e.DELETE("/api/order/customer/:id", handler.DeleteByCustomerId)
	e.POST("/api/order", handler.Post)
	e.PUT("/api/order", handler.Put)

	//For product
	env = os.Getenv("GO_ENV")
	configs.SetConfig(env + "Product")
	config = configs.GetConfig()
	client, err = database.GetClient(config.MongoConnectionUri)
	if err != nil {
		panic("Mongo is not reachable")
	}
	collection, _ = database.GetCollection(client, config.Database, config.Collection)
	repoProduct := repositories.NewProductRepository(collection)
	handlerProduct := handlers.NewProductHandler(repoProduct)
	repositories.SetRepos(repo, repoProduct)
	e.GET("/api/product", handlerProduct.GetAll)
	e.GET("/api/product/:id", handlerProduct.GetById)
	e.DELETE("/api/product/:id", handlerProduct.Delete)
	e.POST("/api/product", handlerProduct.Post)
	e.PUT("/api/product", handlerProduct.Put)
	e.GET("/swagger/*", echoSwagger.WrapHandler)
	e.Start(":5001")
}
