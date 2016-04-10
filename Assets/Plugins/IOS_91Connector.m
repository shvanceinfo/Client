//
//  LsSdkConector.m
//  unityplusfor91sdk
//
//  Created by lisind on 13-6-28.
//  Copyright (c) 2013年 李思. All rights reserved.
//

#import "IOS_91Connector.h"
#import <NdComPlatform/NdComPlatformAPIResponse.h>
#import <NdComPlatform/NdCPNotifications.h>
#import <NdComPlatform/NdComPlatform.h>

int g_n32AppID = 101601;
const char *g_cpszAppKey = "4d4cdd206eafdf688359ba10c92e5447aac6937c3722e3ee";

#if defined(__cplusplus)
extern "C"{
#endif
    extern void UnitySendMessage(const char *, const char *, const char *);
    extern NSString* CreateNSString (const char* string);
#if defined(__cplusplus)
}
#endif


@implementation CIOS_91Conector

//设置调试模式
-(void)LsNdSetDebugMode
{
    NSLog(@"-----LsNdSetDebugMode-----");
    //UnitySendMessage("Logic", "CPlusNotify", "Message from LsNdSetDebugMode!");
    NdComPlatform *psPlatform = [NdComPlatform defaultPlatform];
    [psPlatform NdSetDebugMode:0];
}

//初始化SDK
-(void)LsNdInit:(int)appid appkey:(NSString*)appkey
{
    //[[NdComPlatform defaultPlatform] NdSetScreenOrientation:UIInterfaceOrientationPortrait];
    NdInitConfigure *cfg = [[[NdInitConfigure alloc]init]autorelease];
    cfg.appid = appid;
    cfg.appKey = appkey;
    [[NdComPlatform defaultPlatform] NdInit:cfg];
    
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(SNSInitResult:) name:(NSString *)kNdCPInitDidFinishNotification object:nil];
}

//初始化更新回调
- (void)SNSInitResult:(NSNotification *)notify
{
    //...执行应用的逻辑,例如登录
    NSLog(@"----SNSInitResult----");
    UnitySendMessage("Logic", "OnSDKNotify", "IOS91Init succed!");
    
    [[NdComPlatform defaultPlatform] NdLoginEx:0];
    
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(SNSLoginResult:) name:(NSString*)kNdCPLoginNotification object:nil];
    
}

-(void) SNSLoginResult:(NSNotification *)notify
{
    NSDictionary *dict = [notify userInfo ];
    BOOL success = [[dict objectForKey:@"result"] boolValue];
    NdGuestAccountStatus * guestStatus = (NdGuestAccountStatus*)[dict objectForKey:@"NdGuestAccountStatus" ];
    
    //登录成功后处理
    if([[ NdComPlatform  defaultPlatform] isLogined] && success) {
        //也可以通过[[NdComPlatform defaultPlatform] getCurrentLoginState]判断是否游客登录状态
        if (guestStatus) {
            if ([guestStatus isGuestLogined]) {
                NSLog(@"Guest login succed");
                //游客账号登录成功;
            }
            else if ([guestStatus isGuestRegistered]) {
                NSLog(@"Guest login to nomal user");
                //游客成功注册为普通账号
            }
        } 
        else   {
            NSLog(@"normal user login success!");
            //普通账号登录成功！
        } 
    }
    //登录失败处理和相应提示
    else {
        int  error = [[dict objectForKey:@"error" ] intValue ];
        NSString * strTip = [NSString  stringWithFormat: @"登录失败, error=%d", error];
        switch  (error)
        {
            case ND_COM_PLATFORM_ERROR_USER_CANCEL ://用户取消登录
                if (([[NdComPlatform  defaultPlatform] getCurrentLoginState] == ND_LOGIN_STATE_GUEST_LOGIN)) {
                    strTip =   @"当前仍处于游客登录状态" ;
                }
                else {
                    strTip = @"用户未登录" ;
                } 
                break ;
            case ND_COM_PLATFORM_ERROR_APP_KEY_INVALID://appId未授权接入, 或appKey 无效
                strTip = @"登录失败, 请检查appId/appKey" ;
                break ;
            case ND_COM_PLATFORM_ERROR_CLIENT_APP_ID_INVALID ://无效的应用ID
                strTip = @"登录失败, 无效的应用ID" ;
                break ;
            case ND_COM_PLATFORM_ERROR_HAS_ASSOCIATE_91 :
                strTip = @"有关联的91账号，不能以游客方式登录";
                break ;
            default:
                //其他类型的错误提示
                break ;
        }
        NSLog(strTip);
    }
}

//进入91平台
-(void)LsNdEnterPlatform
{
    [[NdComPlatform defaultPlatform] NdEnterPlatform:0];
}

@end

#if defined(__cplusplus)
extern "C"{
#endif
    
    NSString* CreateNSString (const char* string)
    {
        if (string)
            return [NSString stringWithUTF8String: string];
        else
            return [NSString stringWithUTF8String: ""];
    }
    
    static CIOS_91Conector *pcIOS91Connector;
    
    void IOS_91SDKConnector_SetDebugMode()
    {
        if(pcIOS91Connector==NULL)
        {
            pcIOS91Connector = [[CIOS_91Conector alloc]init];
        }
        [pcIOS91Connector LsNdSetDebugMode];
    }
    
    void IOS_91SDKConnector_Init()
    {
        if(pcIOS91Connector==NULL)
        {
            pcIOS91Connector = [[CIOS_91Conector alloc]init];
        }
        [pcIOS91Connector LsNdInit:g_n32AppID appkey:CreateNSString(g_cpszAppKey)];
    }
    
    void IOS_91SDKConnector_EnterPlatform()
    {
        if(pcIOS91Connector==NULL)
        {
            pcIOS91Connector = [[CIOS_91Conector alloc]init];
        }
        [pcIOS91Connector LsNdEnterPlatform];
    }

#if defined(__cplusplus)
}
#endif
