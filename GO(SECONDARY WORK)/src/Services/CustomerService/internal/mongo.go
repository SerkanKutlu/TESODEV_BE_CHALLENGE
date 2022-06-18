package internal

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"time"

	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"go.mongodb.org/mongo-driver/mongo/readpref"
)

func GetClient(connectionUri string) (*mongo.Client, error) {
	ctx, cancel := context.WithTimeout(context.Background(), time.Second*10)
	defer cancel() //To free resources
	client, err := mongo.Connect(ctx, options.Client().ApplyURI(connectionUri))
	if err != nil {
		return nil, err
	}
	err = client.Ping(ctx, readpref.Primary())
	if err != nil {
		return nil, err
	}
	return client, nil
}

func GetCollection(client *mongo.Client, dbName, collectionName string) (*mongo.Collection, error) {
	//collection := client.Database(dbName).Collection(collectionName)
	db := client.Database(dbName)
	_, err := db.Collection(collectionName).Indexes().CreateOne(context.Background(),
		mongo.IndexModel{
			Keys:    bson.M{"email": 1},
			Options: options.Index().SetUnique(true),
		})
	if err != nil {
		return nil, err
	}
	return db.Collection(collectionName), nil
}
