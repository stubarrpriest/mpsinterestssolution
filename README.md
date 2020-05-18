# Exploring MPs' financial interests

This repo accompanies a series of articles which aim to add value to the data published by Parliament about MPs' financial interests using Azure and .NET core technologies.

## Infra

Terraform requires a backend to store state and this is the only part of the infrastructure confiugred out of band.  I created the storage using the Azure CLI as follows:
```
az account set --subscription <<SubscriptionId>>
az group create --location uksouth --name bpinfrastorage
az storage account create --name bpinfrastorageact --resource-group bpinfrastorage --location uksouth --sku Standard_LRS
```

