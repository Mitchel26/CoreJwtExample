create procedure [dbo].[SP_User]
@UserId varchar(50),
@Username varchar(50),
@Email varchar(50),
@Password varchar(50),
@OperationType int
as
begin tran
	if(@OperationType = 1) --Insert
	begin
			if exists(select* from [User] where Username=@Username)
			begin
				rollback
					raiserror(N'This username already exit !!!',16,1);
				return
			end
			if exists(select* from [User] where Email=@Email)
				begin
					rollback
						raiserror(N'This email already exit !!!',16,1);
				return
			end

			set @UserId = (select count(*) from [User]) + 1

			insert into [User] (UserId,Username,Email,[Password])
						values(@UserId,@Username,@Email,@Password)
			
			select * from [User] where UserId=@UserId
	end
	else if(@OperationType = 2) --Update
	begin
		if(@UserId = 0)
		begin
			rollback
				raiserror(N'Invalid User !!!', 16,1);
			return
		end
		if exists(select * from [User] where Email=@Email and UserId!=@UserId)
		begin
			rollback
				raiserror(N' This email already exist !!!', 16,1);
			return
		end

		update [User] set Username=@Username,
						  Email=@Email
					where UserId=@UserId

		select * from [User] where UserId=@UserId
	end
	else if(@OperationType=3) --Delete
	begin
		if(@UserId = 0)
		begin
			rollback
				raiserror(N'Invalid User !!!', 16,1);
			return
		end

		delete from [User] where UserId=@UserId
	end
commit tran