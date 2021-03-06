// Package docs GENERATED BY SWAG; DO NOT EDIT
// This file was generated by swaggo/swag
package docs

import "github.com/swaggo/swag"

const docTemplate = `{
    "schemes": {{ marshal .Schemes }},
    "swagger": "2.0",
    "info": {
        "description": "{{escape .Description}}",
        "title": "{{.Title}}",
        "contact": {},
        "version": "{{.Version}}"
    },
    "host": "{{.Host}}",
    "basePath": "{{.BasePath}}",
    "paths": {
        "/order": {
            "get": {
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "List All Orders",
                "responses": {}
            },
            "put": {
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Update Order",
                "parameters": [
                    {
                        "description": "Order",
                        "name": "id",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/types.Order"
                        }
                    }
                ],
                "responses": {}
            },
            "post": {
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Create New Order",
                "parameters": [
                    {
                        "description": "Order",
                        "name": "id",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/types.Order"
                        }
                    }
                ],
                "responses": {}
            }
        },
        "/order/customer/{id}": {
            "get": {
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Get Order By CustomerId",
                "parameters": [
                    {
                        "type": "string",
                        "description": "Customer ID",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {}
            },
            "delete": {
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Delete Order By Customer Id",
                "parameters": [
                    {
                        "type": "string",
                        "description": "Customer Id",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {}
            }
        },
        "/order/status": {
            "put": {
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Change Order Status",
                "parameters": [
                    {
                        "type": "string",
                        "description": "Order ID",
                        "name": "id",
                        "in": "path",
                        "required": true
                    },
                    {
                        "type": "string",
                        "description": "New Status",
                        "name": "status",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {}
            }
        },
        "/order/{id}": {
            "get": {
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Get Order By Id",
                "parameters": [
                    {
                        "type": "string",
                        "description": "Order ID",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {}
            },
            "delete": {
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Delete Order By Id",
                "parameters": [
                    {
                        "type": "string",
                        "description": "Order Id",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {}
            }
        },
        "/product": {
            "get": {
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "List All Products",
                "responses": {}
            },
            "put": {
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Update Product",
                "parameters": [
                    {
                        "description": "Product",
                        "name": "id",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/types.Product"
                        }
                    }
                ],
                "responses": {}
            },
            "post": {
                "consumes": [
                    "application/json"
                ],
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Create New Product",
                "parameters": [
                    {
                        "description": "Product",
                        "name": "id",
                        "in": "body",
                        "required": true,
                        "schema": {
                            "$ref": "#/definitions/types.Product"
                        }
                    }
                ],
                "responses": {}
            }
        },
        "/product/{id}": {
            "get": {
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Get Product By Id",
                "parameters": [
                    {
                        "type": "string",
                        "description": "Product ID",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {}
            },
            "delete": {
                "produces": [
                    "application/json"
                ],
                "tags": [
                    "Order Service"
                ],
                "summary": "Delete Product By Id",
                "parameters": [
                    {
                        "type": "string",
                        "description": "Product Id",
                        "name": "id",
                        "in": "path",
                        "required": true
                    }
                ],
                "responses": {}
            }
        }
    },
    "definitions": {
        "types.Address": {
            "type": "object",
            "properties": {
                "addressLine": {
                    "type": "string"
                },
                "city": {
                    "type": "string"
                },
                "cityCode": {
                    "type": "integer"
                },
                "country": {
                    "type": "string"
                }
            }
        },
        "types.Order": {
            "type": "object",
            "required": [
                "customerId",
                "productIds",
                "status"
            ],
            "properties": {
                "_id": {
                    "description": "calculate auto",
                    "type": "string"
                },
                "address": {
                    "description": "get from customer",
                    "$ref": "#/definitions/types.Address"
                },
                "createdAt": {
                    "description": "calculate auto",
                    "type": "string"
                },
                "customerId": {
                    "description": "from user",
                    "type": "string"
                },
                "productIds": {
                    "description": "from user",
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                },
                "quantity": {
                    "description": "calculate auto",
                    "type": "integer"
                },
                "status": {
                    "description": "from user",
                    "type": "string"
                },
                "total": {
                    "description": "calculate auto",
                    "type": "number"
                },
                "updatedAt": {
                    "description": "calculate auto",
                    "type": "string"
                }
            }
        },
        "types.Product": {
            "type": "object",
            "required": [
                "imageUrl",
                "name",
                "price"
            ],
            "properties": {
                "_id": {
                    "type": "string"
                },
                "imageUrl": {
                    "type": "string"
                },
                "name": {
                    "type": "string"
                },
                "price": {
                    "type": "number"
                }
            }
        }
    }
}`

// SwaggerInfo holds exported Swagger Info so clients can modify it
var SwaggerInfo = &swag.Spec{
	Version:          "1.0.0",
	Host:             "143.198.137.31:5001",
	BasePath:         "/api",
	Schemes:          []string{},
	Title:            "User API Documentation",
	Description:      "",
	InfoInstanceName: "swagger",
	SwaggerTemplate:  docTemplate,
}

func init() {
	swag.Register(SwaggerInfo.InstanceName(), SwaggerInfo)
}
