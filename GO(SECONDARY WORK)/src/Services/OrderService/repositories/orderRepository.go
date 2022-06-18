package repositories

import (
	"OrderService/pkg/customErrors"
	"OrderService/types"
	"context"
	"fmt"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"time"
)

type OrderRepository struct {
	orderCollection *mongo.Collection
}

func NewOrderRepository(collection *mongo.Collection) *OrderRepository {
	return &OrderRepository{
		orderCollection: collection,
	}
}

//Order Repository
func (repo *OrderRepository) FindById(orderId string) (*types.Order, *customErrors.CustomError) {
	filter := bson.M{"_id": orderId}
	var order types.Order
	err := repo.orderCollection.FindOne(context.Background(), filter).Decode(&order)
	if err != nil {
		return nil, customErrors.NotFound
	}
	return &order, nil
}

func (repo *OrderRepository) Create(order *types.Order) (*string, *customErrors.CustomError) {

	result, err := repo.orderCollection.InsertOne(context.Background(), order)
	if err != nil {
		return nil, customErrors.UnknownError
	}
	answer := fmt.Sprintf("%v", result.InsertedID)
	return &answer, nil
}

func (repo *OrderRepository) FindAll() (*[]types.Order, *customErrors.CustomError) {
	cursor, err := repo.orderCollection.Find(context.Background(), bson.M{})
	if err != nil {
		return nil, customErrors.UnknownError
	}
	var orders []types.Order
	var order types.Order
	for cursor.Next(context.TODO()) {
		cursor.Decode(&order)
		orders = append(orders, order)
	}

	return &orders, nil
}

func (repo *OrderRepository) Update(order *types.Order) *customErrors.CustomError {
	result, err := repo.orderCollection.ReplaceOne(context.TODO(), bson.M{"_id": order.Id}, order)
	if err != nil {
		return customErrors.UnknownError
	}
	if result.MatchedCount == 0 {
		return customErrors.NotFound
	}
	return nil
}

func (repo *OrderRepository) Delete(orderId string) *customErrors.CustomError {
	result, err := repo.orderCollection.DeleteOne(context.TODO(), bson.M{"_id": orderId})
	if err != nil || result.DeletedCount == 0 {
		return customErrors.NotFound
	}
	return nil
}

func (repo *OrderRepository) ChangeStatus(orderId string, status string) *customErrors.CustomError {
	filter := bson.M{"_id": orderId}
	var order types.Order
	err := repo.orderCollection.FindOne(context.Background(), filter).Decode(&order)
	if err != nil {
		return customErrors.NotFound
	}
	order.Status = status
	order.UpdatedAt = time.Now().UTC()
	result, err := repo.orderCollection.ReplaceOne(context.TODO(), bson.M{"_id": order.Id}, order)
	if err != nil {
		return customErrors.UnknownError
	}
	if result.MatchedCount == 0 {
		return customErrors.NotFound
	}
	return nil
}

func (repo *OrderRepository) DeleteOrderWithCustomerId(customerId string) *customErrors.CustomError {
	result, err := repo.orderCollection.DeleteMany(context.TODO(), bson.M{"customerId": customerId})
	if err != nil || result.DeletedCount == 0 {
		return customErrors.NotFound
	}
	return nil
}

func (repo *OrderRepository) GetOrdersOfCustomer(customerId string) (*[]types.Order, *customErrors.CustomError) {
	cursor, err := repo.orderCollection.Find(context.Background(), bson.M{"customerId": customerId})
	if err != nil {
		return nil, customErrors.UnknownError
	}
	var orders []types.Order
	var order types.Order
	for cursor.Next(context.TODO()) {
		cursor.Decode(&order)
		orders = append(orders, order)
	}

	return &orders, nil
}
