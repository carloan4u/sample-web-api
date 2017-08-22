data "aws_vpc" "beanstalk_vpc" {
  filter {
    name   = "tag:Name"
    values = ["vpc.eu-west-2"]
  }
}

data "aws_caller_identity" "current" {}

data "aws_subnet_ids" "private" {
  vpc_id = "${data.aws_vpc.beanstalk_vpc.id}"

  tags {
    Name = "private.*"
  }
}

data "aws_subnet_ids" "elb_subnets" {
  vpc_id = "${data.aws_vpc.beanstalk_vpc.id}"

  tags {
    Name = "private.*"
  }
}

variable "app_environment" {
  default = "qa"
}

variable "owner_tag" {
  default = "aws-workshop"
}
