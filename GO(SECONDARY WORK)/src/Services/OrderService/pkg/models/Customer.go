package models

import (
	"OrderService/types"
	"time"
)

type Customer struct {
	Id        string
	Name      string
	Address   types.Address
	Email     string
	CreatedAt time.Time
	UpdatedAt time.Time
}
