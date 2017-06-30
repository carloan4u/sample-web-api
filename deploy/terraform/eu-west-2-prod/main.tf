terraform {
  backend "s3" {
    bucket = "zuto-terraform-state-files"
    key    = "services/sample-web-api-prod.tfstate"
    region = "eu-west-2"
  }
}

provider "aws" {
  region = "eu-west-2"
}

module "beanstalk-web-app" {
  source = "github.com/carloan4u/terraform-aws-beanstalk-environment-module?ref=v1.0.1"
  app_name = "sample-web-api"
  app_environment = "prod"
  asg_min_instances = 1
  asg_max_instances = 3
  extra_tags = {
      Environment = "prod"
  }
}
