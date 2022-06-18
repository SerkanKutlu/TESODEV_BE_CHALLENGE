package handlers

import (
	"OrderService/pkg/customErrors"
	"OrderService/pkg/utils"
	"OrderService/repositories"
	"OrderService/types"
	"github.com/go-playground/validator/v10"
	"github.com/google/uuid"
	"github.com/labstack/echo/v4"
	"net/http"
	"time"
)

type Handler struct {
	repo      *repositories.OrderRepository
	validator *validator.Validate
}

func NewHandler(repo *repositories.OrderRepository) *Handler {

	return &Handler{
		repo:      repo,
		validator: validator.New(),
	}
}

//Order methods
// @Summary      List All Orders
// @Tags         Order Service
// @Produce      json
// @Router       /order [get]
func (handler *Handler) GetAll(c echo.Context) error {
	orders, err := handler.repo.FindAll()
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, orders)
}

//@Summary Get Order By Id
// @Tags         Order Service
// @Param        id   path      string  true  "Order ID"
// @Produce      json
// @Router       /order/{id} [get]
func (handler *Handler) GetById(c echo.Context) error {
	id := c.Param("id")
	order, err := handler.repo.FindById(id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, order)
}

//@Summary Get Order By CustomerId
// @Tags         Order Service
// @Param        id   path      string  true  "Customer ID"
// @Produce      json
// @Router       /order/customer/{id} [get]
func (handler *Handler) GetOrderByCustomerId(c echo.Context) error {
	id := c.Param("id")
	//Customer Check
	httpHelper := &utils.HttpHelper{
		Url: "http://localhost:8000/api/customer/validate/" + id,
	}
	err := httpHelper.ValidateCustomer()
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	orders, err := handler.repo.GetOrdersOfCustomer(id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, orders)
}

//@Summary Delete Order By Id
// @Tags         Order Service
// @Param        id   path      string  true  "Order Id"
// @Produce      json
// @Router       /order/{id} [delete]
func (handler *Handler) Delete(c echo.Context) error {
	id := c.Param("id")
	err := handler.repo.Delete(id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, id)
}

//@Summary Delete Order By Customer Id
// @Tags         Order Service
// @Param        id   path      string  true  "Customer Id"
// @Produce      json
// @Router       /order/customer/{id} [delete]
func (handler *Handler) DeleteByCustomerId(c echo.Context) error {
	id := c.Param("id")
	err := handler.repo.DeleteOrderWithCustomerId(id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, id)
}

//@Summary Create New Order
// @Tags         Order Service
// @Param        id   body      types.Order  true  "Order"
// @Produce      json
// @Accept	 	 json
// @Router       /order [post]
func (handler *Handler) Post(c echo.Context) error {
	var newOrder types.Order
	//Binding body
	if err := c.Bind(&newOrder); err != nil {
		return c.JSON(customErrors.InvalidBody.StatusCode, customErrors.InvalidBody)
	}
	//Validation
	if err := handler.validator.Struct(newOrder); err != nil {
		return c.JSON(customErrors.ValidationFailed.StatusCode, customErrors.ValidationFailed)
	}
	//Customer Check
	httpHelper := &utils.HttpHelper{
		Url: "http://localhost:8000/api/customer/validate/" + newOrder.CustomerId,
	}
	err := httpHelper.ValidateCustomer()
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	//Product Check
	err = utils.CheckProductExist(&newOrder, newOrder.ProductIds)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	//Adding unique id and times
	newOrder.Id = uuid.New().String()
	newOrder.CreatedAt = time.Now().UTC()
	newOrder.UpdatedAt = newOrder.CreatedAt
	//Adding Address
	//Adding address of customer

	//This step can be done when validating customer

	httpHelper.Url = "http://localhost:8000/api/customer/" + newOrder.CustomerId
	customer, err := httpHelper.GetCustomer()
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	newOrder.Address = customer.Address
	result, err := handler.repo.Create(&newOrder)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}

	return c.JSON(http.StatusCreated, result)
}

//@Summary Update Order
// @Tags         Order Service
// @Param        id   body      types.Order  true  "Order"
// @Produce      json
// @Accept	 	 json
// @Router       /order [put]
func (handler *Handler) Put(c echo.Context) error {
	var newOrder types.Order
	if err := c.Bind(&newOrder); err != nil {
		return c.JSON(customErrors.InvalidBody.StatusCode, customErrors.InvalidBody)
	}
	//Validation
	if err := handler.validator.StructExcept(newOrder, "Id"); err != nil {
		return c.JSON(customErrors.ValidationFailed.StatusCode, customErrors.ValidationFailed)
	}

	//Customer Check
	httpHelper := &utils.HttpHelper{
		Url: "http://localhost:8000/api/customer/validate/" + newOrder.CustomerId,
	}
	err := httpHelper.ValidateCustomer()
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	//Product Check
	err = utils.CheckProductExist(&newOrder, newOrder.ProductIds)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	//CreatedAt time
	oldCustomer, err := handler.repo.FindById(newOrder.Id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	newOrder.UpdatedAt = time.Now().UTC()
	newOrder.CreatedAt = oldCustomer.CreatedAt
	//Adding Address
	//Adding address of customer

	//This step can be done when validating customer

	httpHelper.Url = "http://localhost:8000/api/customer/validate/\" + newOrder.CustomerId"
	customer, err := httpHelper.GetCustomer()
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	newOrder.Address = customer.Address
	err = handler.repo.Update(&newOrder)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, newOrder)
}

//@Summary Change Order Status
// @Tags         Order Service
// @Param        id path string true  "Order ID"
// @Param        status path string true  "New Status"
// @Produce      json
// @Accept	 	 json
// @Router       /order/status [put]
func (handler *Handler) ChangeStatus(c echo.Context) error {
	id := c.Param("id")
	status := c.Param("status")
	if err := handler.repo.ChangeStatus(id, status); err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, "")
}

//Product methods
