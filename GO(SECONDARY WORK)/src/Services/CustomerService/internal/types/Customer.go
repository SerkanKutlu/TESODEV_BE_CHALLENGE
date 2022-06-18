package types

import (
	"time"
)

type Customer struct {
	Id        string    `bson:"_id" json:"_id" validate:"isdefault"`
	Name      string    `bson:"name" json:"name" validate:"required"`
	Address   Address   `bson:"address" json:"address" validate:"required"`
	Email     string    `bson:"email" json:"email" validate:"required,email"`
	CreatedAt time.Time `bson:"createdAt" json:"createdAt" validate:"isdefault"`
	UpdatedAt time.Time `bson:"updatedAt" json:"updatedAt" validate:"isdefault"`
}
