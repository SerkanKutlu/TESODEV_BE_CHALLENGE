package handlers

import (
	"OrderService/pkg/customErrors"
	"OrderService/repositories"
	"OrderService/types"
	"github.com/go-playground/validator/v10"
	"github.com/google/uuid"
	"github.com/labstack/echo/v4"
	"net/http"
)

type ProductHandler struct {
	repo      *repositories.ProductRepository
	validator *validator.Validate
}

func NewProductHandler(repo *repositories.ProductRepository) *ProductHandler {
	return &ProductHandler{
		repo:      repo,
		validator: validator.New(),
	}
}

// @Summary      List All Products
// @Tags         Order Service
// @Produce      json
// @Router       /product [get]
func (handler *ProductHandler) GetAll(c echo.Context) error {
	products, err := handler.repo.FindAll()
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, products)
}

//@Summary Get Product By Id
// @Tags         Order Service
// @Param        id   path      string  true  "Product ID"
// @Produce      json
// @Router       /product/{id} [get]
func (handler *ProductHandler) GetById(c echo.Context) error {
	id := c.Param("id")
	product, err := handler.repo.FindById(id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, product)
}

//@Summary Delete Product By Id
// @Tags         Order Service
// @Param        id   path      string  true  "Product Id"
// @Produce      json
// @Router       /product/{id} [delete]
func (handler *ProductHandler) Delete(c echo.Context) error {
	id := c.Param("id")
	err := handler.repo.Delete(id)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, id)
}

//@Summary Create New Product
// @Tags         Order Service
// @Param        id   body      types.Product  true  "Product"
// @Produce      json
// @Accept	 	 json
// @Router       /product [post]
func (handler *ProductHandler) Post(c echo.Context) error {
	var newProduct types.Product
	//Binding body
	if err := c.Bind(&newProduct); err != nil {
		return c.JSON(customErrors.InvalidBody.StatusCode, customErrors.InvalidBody)
	}
	//Validation
	if err := handler.validator.Struct(newProduct); err != nil {
		return c.JSON(customErrors.ValidationFailed.StatusCode, customErrors.ValidationFailed)
	}
	//Adding unique id and times
	newProduct.Id = uuid.New().String()
	result, err := handler.repo.Create(&newProduct)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}

	return c.JSON(http.StatusCreated, result)
}

//@Summary Update Product
// @Tags         Order Service
// @Param        id   body      types.Product  true  "Product"
// @Produce      json
// @Accept	 	 json
// @Router       /product [put]
func (handler *ProductHandler) Put(c echo.Context) error {
	var newProduct types.Product
	if err := c.Bind(&newProduct); err != nil {
		return c.JSON(customErrors.InvalidBody.StatusCode, customErrors.InvalidBody)
	}
	//Validation
	if err := handler.validator.StructExcept(newProduct, "Id"); err != nil {
		return c.JSON(customErrors.ValidationFailed.StatusCode, customErrors.ValidationFailed)
	}
	err := handler.repo.Update(&newProduct)
	if err != nil {
		return c.JSON(err.StatusCode, err)
	}
	return c.JSON(http.StatusOK, newProduct)
}
