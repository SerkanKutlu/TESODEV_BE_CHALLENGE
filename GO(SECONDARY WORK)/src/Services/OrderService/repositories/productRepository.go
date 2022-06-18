package repositories

import (
	"OrderService/types"
	"fmt"
	"github.com/SerkanKutlu/CustomerService/pkg/customErrors"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"golang.org/x/net/context"
)

type ProductRepository struct {
	productCollection *mongo.Collection
}

func NewProductRepository(collection *mongo.Collection) *ProductRepository {
	return &ProductRepository{
		productCollection: collection,
	}
}

func (repo *ProductRepository) FindById(productId string) (*types.Product, *customErrors.CustomError) {
	filter := bson.M{"_id": productId}
	var product types.Product
	err := repo.productCollection.FindOne(context.Background(), filter).Decode(&product)
	if err != nil {
		return nil, customErrors.NotFound
	}
	return &product, nil
}

func (repo *ProductRepository) Create(product *types.Product) (*string, *customErrors.CustomError) {

	result, err := repo.productCollection.InsertOne(context.Background(), product)
	if err != nil {
		return nil, customErrors.UnknownError
	}
	answer := fmt.Sprintf("%v", result.InsertedID)
	return &answer, nil
}

func (repo *ProductRepository) FindAll() (*[]types.Product, *customErrors.CustomError) {
	cursor, err := repo.productCollection.Find(context.Background(), bson.M{})
	if err != nil {
		return nil, customErrors.UnknownError
	}
	var products []types.Product
	var product types.Product
	for cursor.Next(context.TODO()) {
		cursor.Decode(&product)
		products = append(products, product)
	}

	return &products, nil
}

func (repo *ProductRepository) Update(product *types.Product) *customErrors.CustomError {
	result, err := repo.productCollection.ReplaceOne(context.TODO(), bson.M{"_id": product.Id}, product)
	if err != nil {
		return customErrors.UnknownError
	}
	if result.MatchedCount == 0 {
		return customErrors.NotFound
	}
	return nil
}

func (repo *ProductRepository) Delete(productId string) *customErrors.CustomError {
	result, err := repo.productCollection.DeleteOne(context.TODO(), bson.M{"_id": productId})
	if err != nil || result.DeletedCount == 0 {
		return customErrors.NotFound
	}
	return nil
}
