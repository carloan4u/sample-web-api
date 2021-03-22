terraform {
  backend "s3" {
    bucket = "zuto-terraform-state-files"
    key    = "services/sample-test-app/sample-test-app.tfstate"
    region = "eu-west-2"
    acl    = "bucket-owner-full-control"
  }
}

provider "aws" {
  region = "eu-west-2"
}

data "aws_caller_identity" "current" {}

resource "aws_elastic_beanstalk_application" "default" {
  name        = "sample-test-app"
  description = "Sample Application used for AWS workshop"
}

variable "instance_type" {
  default = "t2.small"
}

variable "asg_min_instances" {
  default = 1
}

variable "asg_max_instances" {
  default = 2
}


module "beanstalk-web-app" {
  source                    = "git@github.com:carloan4u/terraform-aws-beanstalk-environment-module.git?ref=v4.2.1"
  app_name                  = aws_elastic_beanstalk_application.default.name
  instance_type             = var.instance_type
  app_environment           = terraform.workspace
  asg_min_instances         = var.asg_min_instances
  asg_max_instances         = var.asg_max_instances
  ec2_key                   = "${terraform.workspace}-ec2-applications"
  create_private_dns_record = true
  owner_tag                 = "IO"
  healthcheck_url           = "/api/status"

  sns_topic = {
    name     = "${aws_elastic_beanstalk_application.default.name}-${terraform.workspace}"
    endpoint = "arn:aws:sqs:eu-west-2:${data.aws_caller_identity.current.account_id}:${aws_elastic_beanstalk_application.default.name}-${terraform.workspace}"
    protocol = "sqs"
  }

  cloudwatch = {
    alarm_topic = "product-integration-sns-topic"
  }
}
