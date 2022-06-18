package internal

import (
	"context"
	"fmt"
	"github.com/SerkanKutlu/CustomerService/internal/types"
	"github.com/SerkanKutlu/CustomerService/pkg/customErrors"

	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
)

type CustomerRepository struct {
	customerCollection *mongo.Collection
}

func NewCustomerRepository(collection *mongo.Collection) *CustomerRepository {
	return &CustomerRepository{
		customerCollection: collection,
	}
}

func (repo *CustomerRepository) FindById(customerId string) (*types.Customer, *customErrors.CustomError) {
	filter := bson.M{"_id": customerId}
	var customer types.Customer
	err := repo.customerCollection.FindOne(context.Background(), filter).Decode(&customer)
	if err != nil {
		return nil, customErrors.NotFound
	}
	return &customer, nil
}

func (repo *CustomerRepository) Create(customer *types.Customer) (*string, *customErrors.CustomError) {

	result, err := repo.customerCollection.InsertOne(context.Background(), customer)
	if err != nil {
		exception := err.(mongo.WriteException)
		errCode := exception.WriteErrors[0].Code
		if errCode == 11000 {
			return nil, customErrors.DuplicatedEmail
		}
		return nil, customErrors.UnknownError
	}
	answer := fmt.Sprintf("%v", result.InsertedID)
	return &answer, nil
}

func (repo *CustomerRepository) FindAll() (*[]types.Customer, *customErrors.CustomError) {
	cursor, err := repo.customerCollection.Find(context.Background(), bson.M{})
	if err != nil {
		return nil, customErrors.UnknownError
	}
	var customers []types.Customer
	var customer types.Customer
	for cursor.Next(context.TODO()) {
		cursor.Decode(&customer)
		customers = append(customers, customer)
	}

	return &customers, nil
}

func (repo *CustomerRepository) Update(customer *types.Customer) *customErrors.CustomError {
	result, err := repo.customerCollection.ReplaceOne(context.TODO(), bson.M{"_id": customer.Id}, customer)
	if err != nil {
		exception := err.(mongo.WriteException)
		errCode := exception.WriteErrors[0].Code
		if errCode == 11000 {
			return customErrors.DuplicatedEmail
		}
		return customErrors.UnknownError
	}
	if result.MatchedCount == 0 {
		return customErrors.NotFound
	}
	return nil
}

func (repo *CustomerRepository) Delete(customerId string) *customErrors.CustomError {
	result, err := repo.customerCollection.DeleteOne(context.TODO(), bson.M{"_id": customerId})
	if err != nil || result.DeletedCount == 0 {
		return customErrors.NotFound
	}
	return nil
}
