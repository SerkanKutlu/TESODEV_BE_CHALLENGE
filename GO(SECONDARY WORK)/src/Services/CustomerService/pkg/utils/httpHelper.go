package utils

import (
	"github.com/SerkanKutlu/CustomerService/pkg/customErrors"
	"net/http"
)

type HttpHelper struct {
	Url string
}

func (helper *HttpHelper) DeleteOrders() *customErrors.CustomError {
	req, err := http.NewRequest("DELETE", helper.Url, nil)
	if err != nil {
		return customErrors.UnknownError
	}
	client := &http.Client{}
	resp, err := client.Do(req)
	if err != nil {
		return customErrors.RemoteServerError
	}
	if resp.StatusCode == 200 || resp.StatusCode == 404 {
		return nil
	}
	return customErrors.RemoteServerError
}
