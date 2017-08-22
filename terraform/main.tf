provider "aws" {
  region = "eu-west-2"
  profile = "zuto-dev"
}

resource "aws_elastic_beanstalk_application" "my_beanstalk_app" {
  name        = "aws-workshop-terraform"
  description = "my super cool beanstalk app"
}

resource "aws_elastic_beanstalk_environment" "beanstalk_environment" {
  name                = "${aws_elastic_beanstalk_application.my_beanstalk_app.name}-${var.app_environment}"
  application         = "${aws_elastic_beanstalk_application.my_beanstalk_app.name}"
  solution_stack_name = "64bit Windows Server 2012 R2 v1.2.0 running IIS 8.5"
  cname_prefix        = "${aws_elastic_beanstalk_application.my_beanstalk_app.name}-${var.app_environment}"
  tags                = "${map("Provisioner", "Terraform","Owner", "${var.owner_tag}", "Application", "${aws_elastic_beanstalk_application.my_beanstalk_app.name}", "Environment", "${var.app_environment}")}"

  setting {
    namespace = "aws:ec2:vpc"
    name      = "VPCId"
    value     = "${data.aws_vpc.beanstalk_vpc.id}"
  }

  setting {
    namespace = "aws:ec2:vpc"
    name      = "AssociatePublicIpAddress"
    value     = "false"
  }

  setting {
    namespace = "aws:ec2:vpc"
    name      = "Subnets"
    value     = "${join(",", data.aws_subnet_ids.private.ids)}"
  }

  setting {
    namespace = "aws:ec2:vpc"
    name      = "ELBSubnets"
    value     = "${join(",", data.aws_subnet_ids.elb_subnets.ids)}"
  }

  setting {
    namespace = "aws:ec2:vpc"
    name      = "ELBScheme"
    value     = "internal"
  }

  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "InstanceType"
    value     = "t2.small"
  }

  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "EC2KeyName"
    value     = "dev-ec2-applications"
  }

  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "IamInstanceProfile"
    value     = "aws-elasticbeanstalk-ec2-role"
  }

  setting {
    namespace = "aws:autoscaling:asg"
    name      = "MinSize"
    value     = "1"
  }

  setting {
    namespace = "aws:autoscaling:asg"
    name      = "MaxSize"
    value     = "2"
  }

  setting {
    namespace = "aws:elasticbeanstalk:environment"
    name      = "ServiceRole"
    value     = "aws-elasticbeanstalk-service-role"
  }

  setting {
    namespace = "aws:elasticbeanstalk:environment"
    name      = "EnvironmentType"
    value     = "LoadBalanced"
  }

  setting {
    namespace = "aws:elasticbeanstalk:application"
    name      = "Application Healthcheck URL"
    value     = "/api/status"
  }

  setting {
    namespace = "aws:elasticbeanstalk:application:environment"
    name      = "environment"
    value     = "${var.app_environment}"
  }

  lifecycle {
    create_before_destroy = true
  }
}
