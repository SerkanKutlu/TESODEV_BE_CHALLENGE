package utils

import (
	"OrderService/pkg/customErrors"
	"OrderService/repositories"
	"OrderService/types"
)

func CheckProductExist(order *types.Order, list []string) *customErrors.CustomError {

	for i := 0; i < len(list); i++ {
		prod, err := repositories.Repos.ProductRepo.FindById(list[i])
		if err != nil {
			return customErrors.ProductNotFound
		}
		order.Quantity++
		order.Total += prod.Price
	}
	return nil
}
