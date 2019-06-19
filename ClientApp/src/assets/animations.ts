import {
  trigger,
  transition,
  state,
  animate,
  animation,
  style,
  keyframes,
  useAnimation,
  animateChild,
  group
} from '@angular/animations';
import { query } from '@angular/core/src/render3/query';

export const expandCollapse = trigger('expandCollapse', [
  state(
    'collapsed',
    style({
      height: 0,
      paddingTop: 0,
      paddingBottom: 0,
      opacity: 0
    })
  ),

  transition('collapsed => expanded', [
    animate(
      '300ms ease-out',
      style({
        height: '*',
        paddingTop: '*',
        paddingBottom: '*'
      })
    ),
    animate('1s', style({ opacity: 1 }))
  ]),

  transition('expanded => collapsed', [animate('300ms ease-in')])
]);

export const expandTable = trigger('expandTable', [
  state(
    'collapsed',
    style({
      height: '145px',
      overflow: 'hidden'
    })
  ),

  transition('collapsed => expanded', [
    animate(
      '300ms ease-out',
      style({
        height: '*',
        overflow: 'auto'
      })
    ),
    animate('1s', style({ opacity: 1 }))
  ]),

  transition('expanded => collapsed', [animate('300ms ease-in')])
]);

export const sideCollapse = trigger('expandCollapse', [
  state(
    'collapsed',
    style({
      width: 0,
      paddingRight: 0,
      paddingLeft: 0
    })
  ),

  transition('collapsed => expanded', [
    animate(
      '300ms ease-out',
      style({
        width: '38%',
        paddingRight: '*',
        paddingLeft: '*'
      })
    ),
    animate('1s', style({ opacity: 1 }))
  ]),

  transition('expanded => collapsed', [animate('300ms ease-in')])
]);

export let bounceOutLeftAnimation = animation(
  animate(
    '0.5s ease-out',
    keyframes([
      style({
        offset: 0.2,
        opacity: 1,
        transform: 'translateX(20px)'
      }),
      style({
        offset: 1,
        opacity: 0,
        transform: 'translateX(-100%)'
      })
    ])
  )
);

export let slideDown = trigger('slideDown', [
  transition(':enter', [
    style({ transform: 'translateY(-100%)' }),
    animate(500)
  ]),

  transition(':leave', [style({ opacity: '0' }), animate(500)])
]);

export let slideDown2 = trigger('slideDown2', [
  transition(':enter', [
    style({
      height: 0,
      paddingTop: 0,
      paddingBottom: 0
    }),
    animate(300)
  ])

  // transition(':leave', [
  //   style({
  //     height: '*',
  //     paddingTop: '*',
  //     paddingBottom: '*',
  //   }),
  //   animate(300)
  // ]),

  //   transition(':leave', [
  //     group([
  //       query('.MenuesContainer', [
  //         style({
  //           height: '*',
  //           paddingTop: '*',
  //           paddingBottom: '*',
  //          }),
  //         animate(300)
  //       ]),
  //       query('.innerMenu', animateChild())
  //     ])

  //   ]
  // )
]);

export let slide = trigger('slide', [
  transition(':enter', [
    style({ transform: 'translateX(20px)' }),
    animate(500)
  ]),

  transition(':leave', useAnimation(bounceOutLeftAnimation))
]);

export let fadeInAnimation = animation(
  [style({ opacity: 0 }), animate('{{ duration }} {{ easing }}')],
  {
    params: {
      duration: '300ms',
      easing: 'ease-out'
    }
  }
);

export let fade = trigger('fade', [
  transition(':enter', useAnimation(fadeInAnimation)),

  transition(':leave', [animate(200, style({ opacity: 0 }))])
]);

export const smoothHeight = trigger('grow', [
  transition('void <=> *', []),
  transition(
    '* <=> *',
    [style({ height: '{{startHeight}}px', opacity: 0 }), animate('.5s ease')],
    {
      params: { startHeight: 0 }
    }
  )
]);
