provider "aws" {
  region = "eu-west-2"
  profile = "zuto-dev"
}

resource "aws_elastic_beanstalk_application" "my_beanstalk_app" {
  name        = "aws-workshop-terraform"
  description = "my super cool beanstalk app"
}

module "beanstalk-web-app" {
  source = "git@github.com:carloan4u/terraform-aws-beanstalk-environment-module.git?ref=v1.2.5"
  app_name = "${aws_elastic_beanstalk_application.my_beanstalk_app.name}"
  app_environment = "${var.app_environment}"
  asg_min_instances = 1
  asg_max_instances = 3
  ec2_key = "dev-ec2-applications"
  owner_tag = "${var.owner_tag}"
  healthcheck_url = "/api/status"

  sns_topic = {
   name = "${aws_elastic_beanstalk_application.my_beanstalk_app.name}-${var.app_environment}"
   endpoint = "arn:aws:sqs:eu-west-2:${data.aws_caller_identity.current.account_id}:${aws_elastic_beanstalk_application.my_beanstalk_app.name}-${var.app_environment}"
   protocol = "sqs"
 }
}
