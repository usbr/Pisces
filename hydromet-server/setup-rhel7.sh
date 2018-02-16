#!/bin/bash
# Initial Hydromet Server setup for RHEL7 
#
# This script installs initial software.
sudo yum -y update
sudo yum install ./files/epel-release-latest-7.noarch.rpm
sudo yum -y install git  ansible

ssh-copy-id -i ~/.ssh/id_rsa.pub root@localhost
