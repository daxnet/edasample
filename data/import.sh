#!/bin/bash
curl -X DELETE http://localhost:5000/products-service
curl -X POST -H "Content-Type:application/json" -d @products.json http://localhost:5000/products-service/create-many
curl -X DELETE http://localhost:5000/customer-service/daxnet
curl -X POST -H "Content-Type:application/json" -d @customer.json http://localhost:5000/customer-service

