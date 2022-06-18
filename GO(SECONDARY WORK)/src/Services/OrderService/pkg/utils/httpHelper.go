package utils

import (
	"OrderService/pkg/customErrors"
	"OrderService/pkg/models"
	"encoding/json"
	"io/ioutil"
	"net/http"
)

type HttpHelper struct {
	Url string
}

func (helper *HttpHelper) ValidateCustomer() *customErrors.CustomError {
	resp, err := http.Get(helper.Url)
	if err != nil {
		return customErrors.RemoteServerError
	}
	if resp.StatusCode == 200 {
		return nil
	}
	return customErrors.CustomerNotFound
}

func (helper *HttpHelper) GetCustomer() (*models.Customer, *customErrors.CustomError) {
	resp, err := http.Get(helper.Url)
	if err != nil {
		return nil, customErrors.RemoteServerError
	}
	if resp.StatusCode == 200 {
		body, err := ioutil.ReadAll(resp.Body)
		if err != nil {
			return nil, customErrors.RemoteServerError
		}
		var customer models.Customer
		err = json.Unmarshal(body, &customer)
		if err != nil {
			return nil, customErrors.RemoteServerError
		}
		return &customer, nil
	}
	return nil, customErrors.CustomerNotFound
}
