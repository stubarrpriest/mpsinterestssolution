terraform {
  backend "azurerm" {
	resource_group_name  ="bpinfrastorage" 
    storage_account_name = "bpinfrastorageact"
    container_name       = "terraform"
    key                  = "mpsinterestsdev.state.tf"
  }
}

provider "azurerm" {
  version = ">2.0.0"
  features {}
}

resource "azurerm_resource_group" "mpsinterests" {
  name     = "MpsInterestsDev"
  location = "uksouth"
}

resource "azurerm_storage_account" "mpsinterests" {
  name                     = "mpsinterestsdevstorage"
  resource_group_name      = azurerm_resource_group.mpsinterests.name
  location                 = azurerm_resource_group.mpsinterests.location
  account_tier             = "Standard"
  account_kind             = "StorageV2"
  account_replication_type = "LRS"
  enable_https_traffic_only = false
  static_website {
    index_document             = "index.html"
  }
  custom_domain{
	name				="firexplorerdev.barrpriestltd.co.uk"
	use_subdomain = false
  }
}

resource "azurerm_application_insights" "mpsinterests" {
  name                = "mpsinterests-dev-appinsights"
  location            = azurerm_resource_group.mpsinterests.location
  resource_group_name = azurerm_resource_group.mpsinterests.name
  application_type    = "web"
}

output "instrumentation_key" {
  value = azurerm_application_insights.mpsinterests.instrumentation_key
}

output "app_id" {
  value = azurerm_application_insights.mpsinterests.app_id
}


resource "azurerm_app_service_plan" "mpsinterests" {
  name                = "azure-functions-mps-interests-service-plan"
  location            = azurerm_resource_group.mpsinterests.location
  resource_group_name = azurerm_resource_group.mpsinterests.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "mpsinterests" {
  name                       = "azure-functions-mps-interests"
  location                   = azurerm_resource_group.mpsinterests.location
  resource_group_name        = azurerm_resource_group.mpsinterests.name
  app_service_plan_id        = azurerm_app_service_plan.mpsinterests.id
  storage_account_name       = azurerm_storage_account.mpsinterests.name
  storage_account_access_key = azurerm_storage_account.mpsinterests.primary_access_key
  app_settings = {
     "APPINSIGHTS_INSTRUMENTATIONKEY" = "${azurerm_application_insights.mpsinterests.instrumentation_key}"
   }
}

resource "azurerm_resource_group" "mpsinterestsprod" {
  name     = "MpsInterestsProd"
  location = "uksouth"
}

resource "azurerm_storage_account" "mpsinterestsprod" {
  name                     = "mpsinterestsprodstorage"
  resource_group_name      = azurerm_resource_group.mpsinterestsprod.name
  location                 = azurerm_resource_group.mpsinterestsprod.location
  account_tier             = "Standard"
  account_kind             = "StorageV2"
  account_replication_type = "LRS"
  enable_https_traffic_only = false
  static_website {
    index_document             = "index.html"
  }
  custom_domain{
	name				="firexplorer.barrpriestltd.co.uk"
	use_subdomain = false
  }
}


