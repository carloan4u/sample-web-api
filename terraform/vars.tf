data "aws_caller_identity" "current" {}

variable "app_name" {}

variable "app_environment" {
  default = "qa"
}

variable "owner_tag" {
  default = "aws-workshop"
}
