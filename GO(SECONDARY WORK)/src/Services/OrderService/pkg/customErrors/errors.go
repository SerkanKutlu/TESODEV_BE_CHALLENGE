package customErrors

import (
	"fmt"
	"net/http"
)

type CustomError struct {
	StatusCode int
	Desc       string
}

func newCustomError(statusCode int, description string) *CustomError {
	return &CustomError{
		Desc:       description,
		StatusCode: statusCode,
	}
}

func (e *CustomError) Error() string {
	return fmt.Sprintf("Description: %s\nStatusCode: %d", e.Desc, e.StatusCode)
}

var (
	UnknownError      = newCustomError(http.StatusInternalServerError, "Internal Server Error")
	NotFound          = newCustomError(http.StatusNotFound, "Entity not found")
	InvalidBody       = newCustomError(http.StatusBadRequest, "Invalid Request Body")
	ValidationFailed  = newCustomError(http.StatusBadRequest, "Validation Failed")
	RemoteServerError = newCustomError(http.StatusServiceUnavailable, "Remote Server Error, Try Again Later")
	CustomerNotFound  = newCustomError(http.StatusNotFound, "Customer not found")
	ProductNotFound   = newCustomError(http.StatusNotFound, "Product not found")
)
