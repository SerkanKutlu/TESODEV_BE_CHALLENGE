package internal

import (
	"github.com/SerkanKutlu/CustomerService/internal/types"
	"github.com/SerkanKutlu/CustomerService/pkg/customErrors"
	"github.com/SerkanKutlu/CustomerService/pkg/utils"
	"github.com/go-playground/validator/v10"
	"net/http"
	"time"

	"github.com/google/uuid"
	"github.com/labstack/echo/v4"
)

type Handler struct {
	repo      *CustomerRepository
	validator *validator.Validate
}

func NewHandler(repo *CustomerRepository) *Handler {

	return &Handler{
		repo:      repo,
		validator: validator.New(),
	}
}

// @Summary      List All Customers
// @Tags         Customer Service
// @Produce      json
// @Router       /customer [get]
func (handler *Handler) GetAll(c echo.Context) error {
	customer, err := handler.repo.FindAll()
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, customer)
}

//@Summary Get Customer By Id
// @Tags         Customer Service
// @Param        id   path      string  true  "Customer ID"
// @Produce      json
// @Router       /customer/{id} [get]
func (handler *Handler) GetById(c echo.Context) error {
	id := c.Param("id")
	customer, err := handler.repo.FindById(id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, customer)
}

//@Summary Validate Customer By Id
// @Tags         Customer Service
// @Param        id   path      string  true  "Customer ID"
// @Produce      json
// @Router       /customer/validate/{id} [get]
func (handler *Handler) Validate(c echo.Context) error {
	id := c.Param("id")
	_, err := handler.repo.FindById(id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, true)
}

//@Summary Delete Customer By Id
// @Tags         Customer Service
// @Param        id   path      string  true  "Customer Id"
// @Produce      json
// @Router       /customer/{id} [delete]
func (handler *Handler) Delete(c echo.Context) error {
	id := c.Param("id")
	err := handler.repo.Delete(id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	httpHelper := &utils.HttpHelper{
		Url: "http://localhost:8001/api/order/customer/" + id,
	}
	err = httpHelper.DeleteOrders()
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, id)
}

//@Summary Create New Customer
// @Tags         Customer Service
// @Param        id   body      types.Customer  true  "Customer"
// @Produce      json
// @Accept	 	 json
// @Router       /customer [post]
func (handler *Handler) Post(c echo.Context) error {
	var newCustomer types.Customer
	//Binding body
	if err := c.Bind(&newCustomer); err != nil {
		return c.JSON(customErrors.InvalidBody.StatusCode, customErrors.InvalidBody)
	}
	//Validation
	if err := handler.validator.Struct(newCustomer); err != nil {
		return c.JSON(customErrors.ValidationFailed.StatusCode, customErrors.ValidationFailed)
	}
	//Adding unique id and times
	newCustomer.Id = uuid.New().String()
	newCustomer.CreatedAt = time.Now().UTC()
	newCustomer.UpdatedAt = newCustomer.CreatedAt
	result, err := handler.repo.Create(&newCustomer)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}

	return c.JSON(http.StatusCreated, result)
}

//@Summary Update Customer
// @Tags         Customer Service
// @Param        id   body      types.Customer  true  "Customer"
// @Produce      json
// @Accept	 	 json
// @Router       /customer [put]
func (handler *Handler) Put(c echo.Context) error {
	var newCustomer types.Customer
	if err := c.Bind(&newCustomer); err != nil {
		return c.JSON(customErrors.InvalidBody.StatusCode, customErrors.InvalidBody)
	}
	//Validation
	if err := handler.validator.StructExcept(newCustomer, "Id"); err != nil {
		return c.JSON(customErrors.ValidationFailed.StatusCode, customErrors.ValidationFailed)
	}
	//CreatedAt time
	oldCustomer, err := handler.repo.FindById(newCustomer.Id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	newCustomer.UpdatedAt = time.Now().UTC()
	newCustomer.CreatedAt = oldCustomer.CreatedAt
	err = handler.repo.Update(&newCustomer)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, newCustomer)
}
