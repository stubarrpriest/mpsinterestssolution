# Infra

az account set --subscription <<SubscriptionId>>

az group create --location uksouth --name bpinfrastorage

az storage account create --name bpinfrastorageact --resource-group bpinfrastorage --location uksouth --sku Standard_LRS

