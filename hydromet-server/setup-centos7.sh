#!/bin/bash
# Initial Hydromet Server setup for centos7
#
echo "This script installs initial software."
echo "hydromet needs sudo permissions"
echo " #visudo   (as root)"
echo " add line:     %hydromet  ALL=(ALL) NOPASSWD: ALL"

sudo yum -y update
sudo yum install epel-release
yum -y install git  ansible

sudo ssh-copy-id -i ~/.ssh/id_rsa.pub root@localhost

