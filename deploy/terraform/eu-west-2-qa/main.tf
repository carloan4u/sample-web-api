terraform {
  backend "s3" {
    bucket = "zuto-terraform-state-files"
    key    = "services/sample-web-api-qa.tfstate"
    region = "eu-west-2"
  }
}

provider "aws" {
  region = "eu-west-2"
}

resource "aws_elastic_beanstalk_application" "default" {
  name = "sample-web-api"
  description = "Sample Application"
}

module "beanstalk-web-app" {
  source = "github.com/carloan4u/terraform-aws-beanstalk-environment-module?ref=v1.0.1"
  app_name = "${aws_elastic_beanstalk_application.default.name}"
  app_environment = "qa"
  asg_min_instances = 1
  asg_max_instances = 2
}
