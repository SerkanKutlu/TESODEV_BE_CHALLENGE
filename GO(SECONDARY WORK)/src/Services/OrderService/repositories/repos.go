package repositories

type Repositories struct {
	OrderRepo   *OrderRepository
	ProductRepo *ProductRepository
}

var Repos Repositories

func SetRepos(orderRepo *OrderRepository, productRepo *ProductRepository) {
	Repos.ProductRepo = productRepo
	Repos.OrderRepo = orderRepo
}
