﻿#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

using System;
using System.Collections.Generic;
#if CORE_CLR
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using MsgPack.Serialization.AbstractSerializers;
#if NETFX_CORE || CORE_CLR
using MsgPack.Serialization.Reflection;
#endif // NETFX_CORE || CORE_CLR

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		An implementation of <see cref="SerializerBuilder{TContext,TConstruct}"/> using expression tree.
	/// </summary>
	internal sealed class ExpressionTreeSerializerBuilder : SerializerBuilder<ExpressionTreeContext, ExpressionConstruct>
	{
		// ReSharper disable once InconsistentNaming
		private static readonly MethodInfo CreateSerializerConstructorCore_1Method =
			typeof( ExpressionTreeSerializerBuilder ).GetRuntimeMethod( "CreateSerializerConstructorCore" );

		/// <summary>
		///		Initializes a new instance of the <see cref="ExpressionTreeSerializerBuilder"/> class.
		/// </summary>
		/// <param name="targetType">The type of serialization target.</param>
		/// <param name="collectionTraits">The collection traits of the serialization target.</param>
		public ExpressionTreeSerializerBuilder( Type targetType, CollectionTraits collectionTraits )
			: base( targetType, collectionTraits ) { }

#if FEATURE_TAP

		protected override bool WithAsync( ExpressionTreeContext context )
		{
#warning TODO: true async
			// Currently, ET serializer does not support true async.
			return false;
		}

#endif // FEATURE_TAP

		protected override ExpressionConstruct MakeNullLiteral( ExpressionTreeContext context, TypeDefinition contextType )
		{
			return Expression.Constant( null, contextType.ResolveRuntimeType() );
		}

		protected override ExpressionConstruct MakeByteLiteral( ExpressionTreeContext context, byte constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeSByteLiteral( ExpressionTreeContext context, sbyte constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeInt16Literal( ExpressionTreeContext context, short constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeUInt16Literal( ExpressionTreeContext context, ushort constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeInt32Literal( ExpressionTreeContext context, int constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeUInt32Literal( ExpressionTreeContext context, uint constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeInt64Literal( ExpressionTreeContext context, long constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeUInt64Literal( ExpressionTreeContext context, ulong constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeReal32Literal( ExpressionTreeContext context, float constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeReal64Literal( ExpressionTreeContext context, double constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeBooleanLiteral( ExpressionTreeContext context, bool constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeCharLiteral( ExpressionTreeContext context, char constant )
		{
			return Expression.Constant( constant );
		}


		protected override ExpressionConstruct MakeStringLiteral( ExpressionTreeContext context, string constant )
		{
			return Expression.Constant( constant );
		}

		protected override ExpressionConstruct MakeEnumLiteral( ExpressionTreeContext context, TypeDefinition type, object constant )
		{
			return Expression.Constant( constant, type.ResolveRuntimeType() );
		}

		protected override ExpressionConstruct MakeDefaultLiteral( ExpressionTreeContext context, TypeDefinition type )
		{
			return Expression.Default( type.ResolveRuntimeType() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitThisReferenceExpression( ExpressionTreeContext context )
		{
			return context.This;
		}

		protected override ExpressionConstruct EmitBoxExpression( ExpressionTreeContext context, TypeDefinition valueType, ExpressionConstruct value )
		{
			return Expression.Convert( value, typeof( object ) );
		}

		protected override ExpressionConstruct EmitUnboxAnyExpression( ExpressionTreeContext context, TypeDefinition targetType, ExpressionConstruct value )
		{
			return Expression.Convert( value, targetType.ResolveRuntimeType() );
		}

		protected override ExpressionConstruct EmitNotExpression( ExpressionTreeContext context, ExpressionConstruct booleanExpression )
		{
			return Expression.Not( booleanExpression );
		}

		protected override ExpressionConstruct EmitEqualsExpression(
			ExpressionTreeContext context, ExpressionConstruct left, ExpressionConstruct right
		)
		{
			return Expression.Equal( left, right );
		}

		protected override ExpressionConstruct EmitNotEqualsExpression(
			ExpressionTreeContext context, ExpressionConstruct left, ExpressionConstruct right
		)
		{
			return Expression.NotEqual( left, right );
		}

		protected override ExpressionConstruct EmitGreaterThanExpression(
			ExpressionTreeContext context, ExpressionConstruct left, ExpressionConstruct right
		)
		{
			return Expression.GreaterThan( left, right );
		}

		protected override ExpressionConstruct EmitLessThanExpression(
			ExpressionTreeContext context, ExpressionConstruct left, ExpressionConstruct right
		)
		{
			return Expression.LessThan( left, right );
		}

		protected override ExpressionConstruct EmitIncrement( ExpressionTreeContext context, ExpressionConstruct int32Value )
		{
			return Expression.Assign( int32Value, Expression.Increment( int32Value ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitTypeOfExpression( ExpressionTreeContext context, TypeDefinition type )
		{
			if ( SerializerDebugging.DumpEnabled )
			{
				// LambdaExpression.CompileToMethod cannot handle RuntimeTypeHandle, but can handle Type constants.
				return Expression.Constant( type.ResolveRuntimeType() );
			}
			else
			{
				// WinRT expression tree cannot handle Type constants, but handle RuntimeTypeHandle.
				return Expression.Call( Metadata._Type.GetTypeFromHandle, Expression.Constant( type.ResolveRuntimeType().TypeHandle ) );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitMethodOfExpression( ExpressionTreeContext context, MethodBase method )
		{
			if ( SerializerDebugging.DumpEnabled )
			{
				// LambdaExpression.CompileToMethod cannot handle RuntimeTypeHandle, but can handle Type constants.
				return Expression.Constant( method );
			}
			else
			{
#if !NETFX_CORE && !CORE_CLR
				// WinRT expression tree cannot handle Type constants, but handle RuntimeTypeHandle.
#if DEBUG
				Contract.Assert( method.DeclaringType != null, "method.DeclaringType != null" );
#endif // DEBUG
				return Expression.Call( Metadata._MethodBase.GetMethodFromHandle, Expression.Constant( method.MethodHandle ), Expression.Constant( method.DeclaringType.TypeHandle ) );
#else
				// WinRT expression tree cannot handle Type constants, and MethodHandle property is not exposed.
				// typeof( T ).GetRuntimeMethod( method.Name, ...{types}... );
				return
					Expression.Call(
						ReflectionHelpers.GetRuntimeMethodMethod,
						this.EmitTypeOfExpression( context, method.DeclaringType ).Expression,
						Expression.Constant( method.Name ),
						Expression.NewArrayInit(
							typeof( Type ),
							method.GetParameters().Select( p => this.EmitTypeOfExpression( context, p.ParameterType ).Expression )
						)
					);
#endif // !NETFX_CORE && !CORE_CLR
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitFieldOfExpression( ExpressionTreeContext context, FieldInfo field )
		{
			if ( SerializerDebugging.DumpEnabled )
			{
				// LambdaExpression.CompileToMethod cannot handle RuntimeTypeHandle, but can handle Type constants.
				return Expression.Constant( field );
			}
			else
			{
#if !NETFX_CORE && !CORE_CLR
#if DEBUG
				Contract.Assert( field.DeclaringType != null, "field.DeclaringType != null" );
#endif // DEBUG
				return Expression.Call( Metadata._FieldInfo.GetFieldFromHandle, Expression.Constant( field.FieldHandle ), Expression.Constant( field.DeclaringType.TypeHandle ) );
#else
				// WinRT expression tree cannot handle Type constants, and MethodHandle property is not exposed.
				// typeof( T ).GetRuntimeField( field.Name );
				return
					Expression.Call(
						ReflectionHelpers.GetRuntimeFieldMethod,
						this.EmitTypeOfExpression( context, field.DeclaringType ).Expression,
						Expression.Constant( field.Name )
					);
#endif // !NETFX_CORE && !CORE_CLR
			}
		}

		protected override ExpressionConstruct EmitSequentialStatements( ExpressionTreeContext context, TypeDefinition contextType, IEnumerable<ExpressionConstruct> statements )
		{
			var sts = statements.Where( s => s != null ).ToArray();
#if DEBUG
			Contract.Assert(
				contextType.ResolveRuntimeType().IsAssignableFrom( sts.Last().ContextType.ResolveRuntimeType() ),
				sts.Last().ContextType.ResolveRuntimeType()  + " is " + contextType.ResolveRuntimeType()
			);
#endif // DEBUG

			return
				Expression.Block(
					contextType.ResolveRuntimeType(),
					sts.Select( c => c.Expression ).OfType<ParameterExpression>().Distinct(), // For declare and re-refer pattern
					sts.Where( c => c.IsSignificantReference || !( c.Expression is ParameterExpression ) ).Select( c => c.Expression )
				);
		}

		protected override ExpressionConstruct DeclareLocal(
			ExpressionTreeContext context, TypeDefinition nestedType, string name
		)
		{
			return Expression.Variable( nestedType.ResolveRuntimeType(), name );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override ExpressionConstruct ReferArgument( ExpressionTreeContext context, TypeDefinition type, string name, int index )
		{
			return context.GetCurrentParameters()[ index ];
		}

		protected override ExpressionConstruct EmitCreateNewObjectExpression(
			ExpressionTreeContext context, ExpressionConstruct variable, ConstructorDefinition constructor, params ExpressionConstruct[] arguments
		)
		{
			return Expression.New( constructor.ResolveRuntimeConstructor(), arguments.Select( c => c.Expression ) );
		}

		protected override ExpressionConstruct EmitInvokeVoidMethod(
			ExpressionTreeContext context, ExpressionConstruct instance, MethodDefinition method, params ExpressionConstruct[] arguments
		)
		{
			return this.EmitInvokeMethodExpression( context, instance, method, arguments );
		}

		protected override ExpressionConstruct EmitInvokeMethodExpression(
			ExpressionTreeContext context, ExpressionConstruct instance, MethodDefinition method, IEnumerable<ExpressionConstruct> arguments
		)
		{
			if ( method.TryResolveRuntimeMethod() == null )
			{
#if DEBUG
				Contract.Assert( context.GetMethodLambda( method.MethodName ) != null, "No " + method.MethodName );
#endif // DEBUG
				// Building private methods
				return
					Expression.Invoke(
						context.GetMethodLambda( method.MethodName ),
						context.GetCommonThisMethodArguments( method.MethodName ).Concat( arguments.Select( a => a.Expression ) )
					);
			}

			return
				instance == null
					? Expression.Call( method.ResolveRuntimeMethod(), arguments.Select( c => c.Expression ) )
					: Expression.Call( instance, method.ResolveRuntimeMethod(), arguments.Select( c => c.Expression ) );
		}

		protected override ExpressionConstruct EmitInvokeDelegateExpression( ExpressionTreeContext context, TypeDefinition delegateReturnType, ExpressionConstruct @delegate, params ExpressionConstruct[] arguments )
		{
			return Expression.Call( @delegate, @delegate.ContextType.ResolveRuntimeType().GetMethod( "Invoke" ), arguments.Select( c => c.Expression ) );
		}

		protected override ExpressionConstruct EmitGetPropertyExpression(
			ExpressionTreeContext context, ExpressionConstruct instance, PropertyInfo property
		)
		{
			return Expression.Property( instance, property );
		}

		protected override ExpressionConstruct EmitGetFieldExpression( ExpressionTreeContext context, ExpressionConstruct instance, FieldDefinition field )
		{
			return Expression.Field( instance, field.ResolveRuntimeField() );
		}

		protected override ExpressionConstruct EmitSetProperty(
			ExpressionTreeContext context, ExpressionConstruct instance, PropertyInfo property, ExpressionConstruct value
		)
		{
			return Expression.Assign( Expression.Property( instance, property ), value );
		}
		protected override ExpressionConstruct EmitSetIndexedProperty(
			ExpressionTreeContext context, ExpressionConstruct instance, TypeDefinition declaringType, string proeprtyName, ExpressionConstruct key, ExpressionConstruct value
		)
		{
#if DEBUG
			Contract.Assert( declaringType.HasRuntimeTypeFully() );
			Contract.Assert( key.ContextType.HasRuntimeTypeFully() );
			Contract.Assert( value.ContextType.HasRuntimeTypeFully() );
#endif
			var indexer =
				declaringType.ResolveRuntimeType().GetProperties().Single(
					p => p.Name == proeprtyName
						&& p.GetSetMethod().GetParameters().Length == 2
						&& p.GetSetMethod().GetParameters()[ 0 ].ParameterType == key.ContextType.ResolveRuntimeType()
						&& p.GetSetMethod().GetParameters()[ 1 ].ParameterType == value.ContextType.ResolveRuntimeType()
				);
			return Expression.Assign( Expression.Property( instance, indexer, key ), value );
		}

		protected override ExpressionConstruct EmitSetField(
			ExpressionTreeContext context, ExpressionConstruct instance, FieldDefinition field, ExpressionConstruct value
		)
		{
			return Expression.Assign( Expression.Field( instance, field.ResolveRuntimeField() ), value );
		}

		protected override ExpressionConstruct EmitSetField(
			ExpressionTreeContext context, ExpressionConstruct instance, TypeDefinition nestedType, string fieldName, ExpressionConstruct value
		)
		{
			return Expression.Assign( Expression.Field( instance, nestedType.ResolveRuntimeType(), fieldName ), value );
		}

		protected override ExpressionConstruct EmitLoadVariableExpression( ExpressionTreeContext context, ExpressionConstruct variable )
		{
			// Just use ParameterExpression.
#if DEBUG
			Contract.Assert(
				( variable.Expression is ParameterExpression ) && variable.ContextType.ResolveRuntimeType() != typeof( void ),
				variable.Expression.ToString()
			);
#endif
			return new ExpressionConstruct( variable, true );
		}

		protected override ExpressionConstruct EmitStoreVariableStatement(
			ExpressionTreeContext context, ExpressionConstruct variable, ExpressionConstruct value
		)
		{
			return Expression.Assign( variable, value );
		}

		protected override ExpressionConstruct EmitTryFinally(
			ExpressionTreeContext context, ExpressionConstruct tryStatement, ExpressionConstruct finallyStatement
		)
		{
			return Expression.TryFinally( tryStatement, finallyStatement );
		}

		protected override ExpressionConstruct EmitCreateNewArrayExpression(
			ExpressionTreeContext context, TypeDefinition elementType, int length
		)
		{
			return Expression.NewArrayBounds( elementType.ResolveRuntimeType(), Expression.Constant( length ) );
		}

		protected override ExpressionConstruct EmitCreateNewArrayExpression(
			ExpressionTreeContext context, TypeDefinition elementType, int length, IEnumerable<ExpressionConstruct> initialElements
		)
		{
			return Expression.NewArrayInit( elementType.ResolveRuntimeType(), initialElements.Select( c => c.Expression ) );
		}

		protected override ExpressionConstruct EmitGetArrayElementExpression( ExpressionTreeContext context, ExpressionConstruct array, ExpressionConstruct index )
		{
			return Expression.ArrayAccess( array, index );
		}

		protected override ExpressionConstruct EmitSetArrayElementStatement( ExpressionTreeContext context, ExpressionConstruct array, ExpressionConstruct index, ExpressionConstruct value )
		{
			return
				Expression.Assign(
					Expression.ArrayAccess( array, index ),
					value
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "2", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitConditionalExpression(
			ExpressionTreeContext context,
			ExpressionConstruct conditionExpression,
			ExpressionConstruct thenExpression,
			ExpressionConstruct elseExpression
		)
		{
#if DEBUG
			Contract.Assert(
				elseExpression == null
				|| thenExpression.ContextType.ResolveRuntimeType() == typeof( void )
				|| elseExpression.ContextType.ResolveRuntimeType() == typeof( void )
				|| thenExpression.ContextType.ResolveRuntimeType() == elseExpression.ContextType.ResolveRuntimeType(),
				thenExpression.ContextType + " != " + ( elseExpression == null ? "(null)" : elseExpression.ContextType.ResolveRuntimeType().FullName )
			);
#endif

			return
				elseExpression == null
				? Expression.IfThen( conditionExpression, thenExpression )
				: ( thenExpression.ContextType.ResolveRuntimeType() == typeof( void ) || elseExpression.ContextType.ResolveRuntimeType() == typeof( void ) )
				? Expression.IfThenElse( conditionExpression, thenExpression, elseExpression )
				: Expression.Condition( conditionExpression, thenExpression, elseExpression );
		}

		protected override ExpressionConstruct EmitAndConditionalExpression(
			ExpressionTreeContext context,
			IList<ExpressionConstruct> conditionExpressions,
			ExpressionConstruct thenExpression,
			ExpressionConstruct elseExpression
		)
		{
			return
				Expression.IfThenElse(
					conditionExpressions.Aggregate( ( l, r ) => Expression.AndAlso( l, r ) ),
					thenExpression,
					elseExpression
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Asserted internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "3", Justification = "Asserted internally" )]
		protected override ExpressionConstruct EmitForEachLoop(
			ExpressionTreeContext context, CollectionTraits collectionTraits, ExpressionConstruct collection, Func<ExpressionConstruct, ExpressionConstruct> loopBodyEmitter
		)
		{
			var enumerator = Expression.Variable( collectionTraits.GetEnumeratorMethod.ReturnType, "enumerator" );
			var current = Expression.Variable( collectionTraits.ElementType, "current" );
			var moveNextMethod = Metadata._IEnumerator.FindEnumeratorMoveNextMethod( enumerator.Type );
			var currentProperty = Metadata._IEnumerator.FindEnumeratorCurrentProperty( enumerator.Type, collectionTraits );

			var endForEach = Expression.Label( "END_FOREACH" );
			return
				Expression.Block(
					new[] { enumerator, current },
					Expression.Assign( enumerator, Expression.Call( collection, collectionTraits.GetEnumeratorMethod ) ),
					Expression.Loop(
						Expression.IfThenElse(
							Expression.Call( enumerator, moveNextMethod ),
							Expression.Block(
								Expression.Assign( current, Expression.Property( enumerator, currentProperty ) ),
								loopBodyEmitter( current )
							),
							Expression.Break( endForEach )
						),
						endForEach
					)
				);
		}

		protected override ExpressionConstruct EmitEnumFromUnderlyingCastExpression(
			ExpressionTreeContext context,
			Type enumType,
			ExpressionConstruct underlyingValue )
		{
			return Expression.Convert( underlyingValue, enumType );
		}

		protected override ExpressionConstruct EmitEnumToUnderlyingCastExpression(
			ExpressionTreeContext context,
			Type underlyingType,
			ExpressionConstruct enumValue )
		{
			// ExpressionTree cannot handle enum to underlying type conversion...
			return Expression.Convert( enumValue, underlyingType );
		}

		protected override ExpressionConstruct EmitNewPrivateMethodDelegateExpression( ExpressionTreeContext context, MethodDefinition method )
		{
			return context.GetMethodLambda( method.MethodName );
		}

		protected override ExpressionConstruct EmitGetPrivateMethodDelegateExpression( ExpressionTreeContext context, MethodDefinition method )
		{
			return this.GetDelegateFieldExpression( context, method );
		}

		protected override ExpressionConstruct EmitGetStaticDelegateExpression( ExpressionTreeContext context, MethodDefinition method )
		{
			// ensure delegate for static API is bookkeeped.
			context.EnsureDelegateForStaticMethodRegistered( method );

			return this.GetDelegateFieldExpression( context, method );
		}

		private ExpressionConstruct GetDelegateFieldExpression( ExpressionTreeContext context, MethodDefinition method )
		{
			var delegateType = SerializerBuilderHelper.GetResolvedDelegateType( method.ReturnType, method.ParameterTypes );

			return
				Expression.Convert(
					Expression.Property(
						Expression.Property(
							context.This,
							"Delegates"
						),
						ExpressionTreeSerializerBuilderHelpers.DelegatesIndexer,
						Expression.Constant( method.MethodName )
					),
					delegateType
				);
		}

		protected override TypeDefinition GetPackOperationType( ExpressionTreeContext context, bool isAsync )
		{
			return typeof( Action<,,,> ).MakeGenericType( typeof( ExpressionCallbackMessagePackSerializer<> ).MakeGenericType( this.TargetType ), typeof( SerializationContext ), typeof( Packer ), this.TargetType );
		}

		protected override TypeDefinition GetUnpackOperationType( ExpressionTreeContext context, bool isAsync )
		{
			return
				TypeDefinition.GenericReferenceType(
					typeof( Action<,,,,,> ),
					typeof( ExpressionCallbackMessagePackSerializer<> ).MakeGenericType( this.TargetType ),
					typeof( SerializationContext ),
					typeof( Unpacker ),
					context.UnpackingContextType ?? this.TargetType,
					typeof( int ),
					typeof( int )
				);
		}

		protected override ExpressionConstruct EmitGetActionsExpression( ExpressionTreeContext context, ActionType action, bool isAsync )
		{
			switch ( action )
			{
				case ActionType.PackToArray:
				{
					return
						Expression.Property(
							context.This,
							context.This.ContextType.ResolveRuntimeType().GetProperty( "PackOperationList" )
						);
				}
				case ActionType.PackToMap:
				{
					return
						Expression.Property(
							context.This,
							context.This.ContextType.ResolveRuntimeType().GetProperty( "PackOperationTable" )
						);
				}
				case ActionType.UnpackFromArray:
				{
					return
						Expression.Property(
							context.This,
							context.This.ContextType.ResolveRuntimeType().GetProperty( "UnpackOperationList" )
						);
				}
				case ActionType.UnpackFromMap:
				{
					return
						Expression.Property(
							context.This,
							context.This.ContextType.ResolveRuntimeType().GetProperty( "UnpackOperationTable" )
						);
				}
				case ActionType.UnpackTo:
				{
					return
						Expression.Property(
							context.This,
							context.This.ContextType.ResolveRuntimeType().GetProperty( "UnpackToAction" ) // "Action" suffix avoids naming conflict
						);
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "action" );
				}
			}
		}

		protected override ExpressionConstruct EmitGetMemberNamesExpression( ExpressionTreeContext context )
		{
			return
				Expression.Property(
					context.This,
					context.This.ContextType.ResolveRuntimeType().GetProperty( "MemberNames" )
				);
		}

		protected override ExpressionConstruct EmitFinishFieldInitializationStatement( ExpressionTreeContext context, string name, ExpressionConstruct value )
		{
			// Make significant reference.
			return new ExpressionConstruct( value.Expression, true );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override Func<SerializationContext, MessagePackSerializer> CreateSerializerConstructor(
			ExpressionTreeContext codeGenerationContext, SerializationTarget targetInfo, PolymorphismSchema schema
		)
		{
			codeGenerationContext.Finish();
			var factory =
				( Func<ExpressionTreeContext, SerializationTarget, PolymorphismSchema, Func<SerializationContext, MessagePackSerializer>> )
					CreateSerializerConstructorCore_1Method.MakeGenericMethod( this.TargetType )
						.CreateDelegate( typeof( Func<ExpressionTreeContext, SerializationTarget, PolymorphismSchema, Func<SerializationContext, MessagePackSerializer>> ), this );
			return factory( codeGenerationContext, targetInfo, schema );

		}

		// ReSharper disable once UnusedMember.Local
		private Func<SerializationContext, MessagePackSerializer> CreateSerializerConstructorCore<TObject>(
			ExpressionTreeContext codeGenerationContext, SerializationTarget targetInfo, PolymorphismSchema schema
		)
		{
			var hasPackOperations = targetInfo != null && !typeof( IPackable ).IsAssignableFrom( typeof( TObject ) );
			var hasUnpackOperations = targetInfo != null && !typeof( IUnpackable ).IsAssignableFrom( typeof( TObject ) );

			return
				ExpressionTreeSerializerBuilderHelpers.CreateFactory(
					codeGenerationContext,
					this.CollectionTraits,
					schema,
					hasPackOperations
						? Expression.Lambda<Func<Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Packer, TObject>[]>>(
							this.EmitPackOperationListInitialization( codeGenerationContext, targetInfo, false ).Expression
						).Compile()()
						: new Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Packer, TObject>[ 0 ],
					hasPackOperations
					&& targetInfo.Members.Any( m => m.Member != null ) // not tuple
						? Expression.Lambda<Func<Dictionary<string, Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Packer, TObject>>>>(
							this.EmitPackOperationTableInitialization( codeGenerationContext, targetInfo, false ).Expression
						).Compile()()
						: new Dictionary<string, Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Packer, TObject>>( 0 ),
					hasUnpackOperations // unpack operation list
						? Expression.Lambda<Func<Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Unpacker, object, int, int>[]>>(
							this.EmitUnpackOperationListInitialization( codeGenerationContext, targetInfo, false ).Expression
						).Compile()()
						: new Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Unpacker, object, int, int>[ 0 ],
					hasUnpackOperations // unpack operation table
						? Expression.Lambda<Func<Dictionary<string, Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Unpacker, object, int, int>>>>(
							this.EmitUnpackOperationTableInitialization( codeGenerationContext, targetInfo, false ).Expression
						).Compile()()
						: new Dictionary<string, Action<ExpressionCallbackMessagePackSerializer<TObject>, SerializationContext, Unpacker, object, int, int>>( 0 ),
					hasUnpackOperations // member names
						? Expression.Lambda<Func<IList<string>>>(
							this.EmitMemberListInitialization( codeGenerationContext, targetInfo ).Expression
							).Compile()()
						: Enumerable.Empty<string>().ToArray()
				);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected override Func<SerializationContext, MessagePackSerializer> CreateEnumSerializerConstructor( ExpressionTreeContext codeGenerationContext )
		{
			codeGenerationContext.Finish();
			// Get at this point to prevent unexpected context change.
			var packUnderyingValueTo = codeGenerationContext.GetDelegate( MethodName.PackUnderlyingValueTo );
			var unpackFromUnderlyingValue = codeGenerationContext.GetDelegate( MethodName.UnpackFromUnderlyingValue );

			var targetType = typeof( ExpressionCallbackEnumMessagePackSerializer<> ).MakeGenericType( this.TargetType );

			return
				context =>
					ReflectionExtensions.CreateInstancePreservingExceptionType<MessagePackSerializer>(
						targetType,
						context,
						EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod(
							context,
							this.TargetType,
							EnumMemberSerializationMethod.Default
						),
						packUnderyingValueTo,
						unpackFromUnderlyingValue
					);
		}

		protected override ExpressionTreeContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context )
		{
			return new ExpressionTreeContext( context, this.TargetType, this.BaseClass );
		}
	}
}
