terraform {
  backend "s3" {
    bucket = "zuto-terraform-state-files"
    key    = "services/rods-application.tfstate"
    region = "eu-west-2"
    acl    = "bucket-owner-full-control"
  }
}

variable "environment" {
  default = "prod"
}

provider "aws" {
  region = "eu-west-2"
}

data "aws_caller_identity" "current" {}

resource "aws_elastic_beanstalk_application" "default" {
  name        = "rods-application"
  description = "Rods even cooler producting app"
}

module "beanstalk-web-app" {
  source            = "git@github.com:carloan4u/terraform-aws-beanstalk-environment-module.git?ref=v1.2.5"
  app_name          = "${aws_elastic_beanstalk_application.default.name}"
  instance_type     = "t2.small"
  app_environment   = "${var.environment}"
  asg_min_instances = 1
  asg_max_instances = 2
  ec2_key           = "prod-ec2-applications"
  create_dns_record = true
  owner_tag         = "R-Calder"
  healthcheck_url   = "/api/status"

  sns_topic = {
    name = "${aws_elastic_beanstalk_application.default.name}-${var.environment}"
    endpoint = "arn:aws:sqs:eu-west-2:${data.aws_caller_identity.current.account_id}:${aws_elastic_beanstalk_application.default.name}-${var.environment}"
    protocol = "sqs"
  }
}